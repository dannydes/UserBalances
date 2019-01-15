using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserBalance.Models;
using UserBalances.Business;
using UserBalances.Models;

namespace UserBalances.Controllers
{
    public class UserController : ApiController
    {
        UserBAL bal = new UserBAL();

        [HttpPut]
        [Route("api/User/Register")]
        public ResultID Register(RegisterUserViewModel model)
        {
            return bal.Register(model);
        }

        [HttpPatch]
        [Route("api/user/update")]
        public ResultID Update(RegisterUserViewModel model)
        {
            return bal.Update(model);
        }

        [HttpPost]
        [Route("api/user/block")]
        public Result Block(UserIDViewModel model)
        {
            return bal.Block(model);
        }

        [HttpPost]
        [Route("api/user/login")]
        public ResultID Login(UserLoginViewModel model)
        {
            return bal.Login(model);
        }

        [HttpPost]
        [Route("api/user/unblock")]
        public Result Unblock(UserIDViewModel model)
        {
            return bal.Unblock(model);
        }

        [HttpPost]
        [Route("api/user/logout")]
        public void LogOut(UserIDViewModel model)
        {
            bal.LogOut(model);
        }

        [HttpPatch]
        [Route("api/user/addfunds")]
        public Result AddFunds(AddFundsViewModel model)
        {
            return bal.AddFunds(model);
        }

        [HttpGet]
        [Route("api/user/getbalance")]
        public ResultFloat GetBalance(Guid userID, string currency = "EUR")
        {
            float balance = bal.GetBalance(userID, currency.ToUpper()).GetAwaiter().GetResult();
            if (balance == -1)
            {
                return new ResultFloat
                {
                    Error = "User not found!"
                };
            }
            else if (balance == -2)
            {
                return new ResultFloat
                {
                    Error = "Request to external API failed!"
                };
            }
            else if (balance == -3)
            {
                return new ResultFloat
                {
                    Error = "Currency not supported!"
                };
            }
            else
            {
                return new ResultFloat
                {
                    Value = balance
                };
            }
        }
    }
}
