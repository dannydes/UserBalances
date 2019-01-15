using System;
using System.Collections.Generic;

namespace UserBalances.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class RegisterUserViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class UserIDViewModel
    {
        public Guid ID { get; set; }
    }

    public class UserLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AddFundsViewModel : UserIDViewModel
    {
        public float Amount { get; set; }
    }
}
