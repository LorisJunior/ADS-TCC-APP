using Firebase.Database;
using Firebase.Database.Query;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;

namespace TCCApp.Services
{
    public class DatabaseService
    {
        static FirebaseClient firebase = new FirebaseClient("https://dbteste-cbb09-default-rtdb.firebaseio.com/");

        public static async Task AddUser(User user)
        {
            //Firebase
            await firebase
               .Child("Users")
                  .PostAsync(user);

            //SQLite
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                db.Insert(GetUser(user));
            }

        }
        public static async Task<List<User>> GetUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  Id = item.Object.Id,
                  Nome = item.Object.Nome,
                  Buffer = item.Object.Buffer,
                  Latitude = item.Object.Latitude,
                  Longitude = item.Object.Longitude
              }).ToList();
        }
        public static async Task<User> GetUser(int Id)
        {
            var allUsers = await GetUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(u => u.Id == Id).FirstOrDefault();
        }
        public static async Task<User> GetUser(User user)
        {
            var allUsers = await GetUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(u => u.Id == user.Id).FirstOrDefault();
        }
        public static async Task UpdateUser(User user)
        {
            //Firebase
            var toUpdateUser = (await firebase
              .Child("Users")
                .OnceAsync<User>())
                   .Where(u => u.Object.Id == user.Id).FirstOrDefault();
            await firebase
              .Child("Users")
                .Child(toUpdateUser.Key)
                  .PutAsync(user);
            //SQLite
            /*using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                db.Update(user);
            }*/
        }
    }
}
