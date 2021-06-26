using AkatsukiDokkanInterrogator.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkatsukiDokkanInterrogator.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IOptions<UserAccount> _userAccountConfiguration;

        public UserAccountService(IOptions<UserAccount> userAccountConfiguration)
        {
            _userAccountConfiguration = userAccountConfiguration;
        }

        public UserAccount Get() => _userAccountConfiguration.Value;
    }
}
