using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;

namespace TCCApp.Services
{
    class FirebaseService
    {
        static FirebaseClient firebase = new FirebaseClient("https://dbteste-cbb09-default-rtdb.firebaseio.com/");

        public static async Task AddUser(User user)
        {
            await firebase
               .Child("Users")
                  .PostAsync(user);
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
              .Child("User")
              .OnceAsync<User>();
            return allUsers.Where(u => u.Id == Id).FirstOrDefault();
        }
    }
}
