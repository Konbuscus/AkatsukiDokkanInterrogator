using AkatsukiDokkanInterrogator.Configurations;
using AkatsukiDokkanInterrogator.Models;
using AkatsukiDokkanInterrogator.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AkatsukiDokkanInterrogator.Helpers
{
    public class FetcherService : IFetcherService
    {
        private readonly IUserAccountService _userAccountService;
        private readonly HttpClient _httpClient;
        public FetcherService(IUserAccountService userAccountService, HttpClient httpClient)
        {
            _userAccountService = userAccountService;
            _httpClient = httpClient;
        }

        public GlobalServer GetParamsForInterrogation(IOptions<GlobalServer> globalConfiguration) => globalConfiguration.Value;

        public string CreateMacAuthenticationHeaders(GlobalServer globalServerInfos)
        {
            var config = GenerateUniqueIdAndAdId();
            var timestamp = DateTimeOffset.Now.TimeOfDay.ToString();
            var nonce = timestamp + ":" + config.First().Value;

            var user = _userAccountService.Get();
            user.AdId = config.First().Key;
            user.UniqueId = config.First().Value;

            var value =  timestamp + '\n' + nonce + '\n' + globalServerInfos.Method + '\n' + globalServerInfos.Action + '\n'
            + globalServerInfos.Host + '\n' + globalServerInfos.Port;

            var hmac = GetHmacFromSecretAndValue("X", value);

            var mac = Base64EncodeDecodeResult(hmac);

            return $"MAC id='{"X"}' nonce='{nonce}' ts='{timestamp}' mac='{mac}'";

        }
        public Dictionary<string, string> GenerateUniqueIdAndAdId()
        {
            string uuid = Guid.NewGuid().ToString();
            string uniqueId = Guid.NewGuid().ToString() + ":" + uuid.Substring(0, 8);
            return new Dictionary<string, string>() { { uuid, uniqueId } };
        }

        public string GetHmacFromSecretAndValue(string secret, string value)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            Byte[] textBytes = encoding.GetBytes(value);
            Byte[] keyBytes = encoding.GetBytes(secret);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public string Base64EncodeDecodeResult(string data) => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data))));

        public async Task<string> SignInToAkatsukiServer(string uri, UserAccount account)
        {
            var jsonUser = JsonConvert.SerializeObject(account);

            _httpClient.BaseAddress = new Uri(uri);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0");
            _httpClient.DefaultRequestHeaders.Add("X-Platform", "global");
            _httpClient.DefaultRequestHeaders.Add("X-ClientVersion", "////");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.PostAsync(uri, new StringContent(jsonUser)).Result.Content.ReadAsStringAsync();

            return response;
            //Captcha solver later
        }
    }
}
