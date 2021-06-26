namespace AkatsukiDokkanInterrogator.Helpers
{
    using AkatsukiDokkanInterrogator.Configurations;
    using AkatsukiDokkanInterrogator.Models;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFetcherService
    {
        GlobalServer GetParamsForInterrogation(IOptions<GlobalServer> globalConfig);

        string CreateMacAuthenticationHeaders(GlobalServer globalServerInfos);

        Dictionary<string, string> GenerateUniqueIdAndAdId();

        Task<string> SignInToAkatsukiServer(string uri, UserAccount userAccount);
    }
}