namespace AkatsukiDokkanInterrogator.Controllers
{
    using AkatsukiDokkanInterrogator.Configurations;
    using AkatsukiDokkanInterrogator.Helpers;
    using AkatsukiDokkanInterrogator.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.IO;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class AkatsukiFetchingController : ControllerBase
    {
        private readonly IOptions<GlobalServer> _globalConfiguration;
        private readonly IOptions<UserAccount> _userAccountConfiguration;
        private readonly IFetcherService _fetcherService;
        public AkatsukiFetchingController(IOptions<GlobalServer> globalConfiguration, IOptions<UserAccount> userAccount, IFetcherService fetcherService)
        {
            _globalConfiguration = globalConfiguration;
            _fetcherService = fetcherService;
            _userAccountConfiguration = userAccount;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var t = await _fetcherService.SignInToAkatsukiServer(_globalConfiguration.Value.Host + "/auth/sign_up", _userAccountConfiguration.Value);

            return t;
        }

        public Task<Stream> Get2()
        {

            _fetcherService.CreateMacAuthenticationHeaders(_fetcherService.GetParamsForInterrogation(_globalConfiguration));


            return Task.FromResult<Stream>(null);
        }
    }
}
