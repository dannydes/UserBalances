using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using UserBalance.Models;
using UserBalances.Models;
using UserBalances.Repository;

namespace UserBalances.Business
{
    public class UserBAL
    {
        private UserRepository repo = new UserRepository();

        public ResultID Register(RegisterUserViewModel user)
        {
            ResultID validity = IsValid(user);
            if (!string.IsNullOrEmpty(validity.Error))
            {
                return validity;
            }

            try
            {
                return new ResultID
                {
                    ID = repo.Register(user.Name, user.Surname, user.Address, user.Country, HashPassword(user.Password), user.Username)
                };
            }
            catch (Exception ex)
            {
                return new ResultID
                {
                    Error = "Failed to register user!"
                };
            }
        }

        public ResultID Update(RegisterUserViewModel user)
        {
            ResultID validity = IsValid(user);
            if (!string.IsNullOrEmpty(validity.Error))
            {
                return validity;
            }

            try
            {
                return new ResultID
                {
                    ID = repo.Update(user.ID, user.Name, user.Surname, user.Address, user.Country, HashPassword(user.Password), user.Username)
                };
            }
            catch (Exception ex)
            {
                return new ResultID
                {
                    Error = "Failed to update user!"
                };
            }
        }

        public Result Block(UserIDViewModel user)
        {
            if (!repo.Block(user.ID)) {
                return new Result
                {
                    Error = "User already blocked!"
                };
            }
            else
            {
                return new Result { };
            }
        }

        public ResultID Login(UserLoginViewModel user)
        {
            Guid userID = repo.Login(user.Username, HashPassword(user.Password));

            if (userID == Guid.Empty)
            {
                return new ResultID
                {
                    Error = "Unable to login"
                };
            }
            else
            {
                return new ResultID
                {
                    ID = userID
                };
            }
        }

        public Result Unblock(UserIDViewModel user)
        {
            if (!repo.Unblock(user.ID))
            {
                return new Result
                {
                    Error = "User not blocked!"
                };
            }
            else
            {
                return new Result { };
            }
        }

        public void LogOut(UserIDViewModel user)
        {
            repo.LogOut(user.ID);
        }

        public Result AddFunds(AddFundsViewModel user)
        {
            float balance = repo.GetBalance(user.ID);

            if (balance == -1)
            {
                return new Result
                {
                    Error = "User not found!"
                };
            }

            float sum = balance + user.Amount;

            if (sum < 0)
            {
                return new Result
                {
                    Error = "Balance not enough!"
                };
            }

            repo.SaveNewBalance(user.ID, sum);

            return new Result { };
        }

        private ResultID IsValid(RegisterUserViewModel user)
        {
            if (user.Name.Length > 20)
            {
                return new ResultID
                {
                    Error = "Name may not be longer than 20 letters."
                };
            }
            else if (!ContainsOnlyLetters(user.Name))
            {
                return new ResultID
                {
                    Error = "Name may only contain letters."
                };
            }
            else if (user.Surname.Length > 20)
            {
                return new ResultID
                {
                    Error = "Surname may not be longer than 20 letters."
                };
            }
            else if (!ContainsOnlyLetters(user.Surname))
            {
                return new ResultID
                {
                    Error = "Surname may only contain letters."
                };
            }
            else if (user.Address.Length > 100)
            {
                return new ResultID
                {
                    Error = "Address may not be longer than 100 characters."
                };
            }
            else if (user.Country.Length > 50)
            {
                return new ResultID
                {
                    Error = "Country may not be longer than 50 letters."
                };
            }
            else if (!ContainsOnlyLetters(user.Country))
            {
                return new ResultID
                {
                    Error = "Country may only contain letters."
                };
            }
            else if (user.Password.Length < 5 || user.Password.Length > 12)
            {
                return new ResultID
                {
                    Error = "Password should be between 5 and 12 characters."
                };
            }
            else if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Surname) || string.IsNullOrEmpty(user.Address)
                || string.IsNullOrEmpty(user.Country))
            {
                return new ResultID
                {
                    Error = "All fields should be filled."
                };
            }

            return new ResultID { };
        }

        private bool ContainsOnlyLetters(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z]+$");
        }

        private string HashPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            return Encoding.ASCII.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }

        public async System.Threading.Tasks.Task<float> GetBalance(Guid userId, string currency)
        {
            float balance = repo.GetBalance(userId);

            if (currency == "EUR")
            {
                return balance;
            }
            else if (currency == "USD" || currency == "GBP")
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage res = await client.GetAsync("http://data.fixer.io/api/latest?access_key=e24034345400940279f8d47c9ae363f9&symbols=USD,GBP&format=1");
                if (res.IsSuccessStatusCode)
                {
                    return balance * (await res.Content.ReadAsAsync<dynamic>().Result.rates[currency]);
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return -3;
            }
        }
    }
}