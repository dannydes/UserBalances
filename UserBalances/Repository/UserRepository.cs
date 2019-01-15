using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserBalances.Models;

namespace UserBalances.Repository
{
    public class UserRepository
    {
        private Models.UserBalancesEntities db = new Models.UserBalancesEntities();

        public Guid Register(string name, string surname, string address, string country, string password, string username)
        {
            Guid userID = Guid.NewGuid();

            db.Users.Add(new Models.User
            {
                UserID = userID,
                FirstName = name,
                LastName = surname,
                Address = address,
                Coutry = country,
                Password = password,
                Balance = 0,
                Username = username
            });
            db.SaveChanges();

            return userID;
        }

        public Guid Update(Guid id, string name, string surname, string address, string country, string password, string username)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);

            user.FirstName = name;
            user.LastName = surname;
            user.Address = address;
            user.Coutry = country;
            user.Password = password;
            user.Username = username;

            db.SaveChanges();

            return id;
        }

        public bool Block(Guid id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);

            if (user != null && user.Blocked == false || user.Blocked == null)
            {
                user.Blocked = true;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public Guid Login(string username, string password) {
            User user = db.Users.FirstOrDefault(u => u.Password == password && u.Username == username);

            if (user == null || user.Blocked == true)
            {
                return Guid.Empty;
            }
            else
            {
                user.LoggedIn = true;
                db.SaveChanges();
                return user.UserID;
            }
        }

        public bool Unblock(Guid id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);

            if (user != null && user.Blocked == true)
            {
                user.Blocked = false;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public void LogOut(Guid id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);

            if (user != null)
            {
                user.LoggedIn = false;
                db.SaveChanges();
            }
        }

        public void SaveNewBalance(Guid id, float balance)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);

            if (user != null)
            {
                user.Balance = balance;
                db.SaveChanges();
            }
        }

        public float GetBalance(Guid id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id && u.LoggedIn == true);
            return user != null ? user.Balance : -1;
        }
    }
}