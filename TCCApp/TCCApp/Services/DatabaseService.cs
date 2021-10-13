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

        
        public async static Task<bool> AddUserAsync(User user)
        {
            try
            {
                user.Key = await GetKey();
                await UpdateUserAsync(user.Key, user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        private async static Task<string> GetKey()
        {
            //Esse método cria um documento vazio e retorna uma chave
            var doc = await firebase
               .Child("Users")
                  .PostAsync(new User());
            return doc.Key;
        }
        public async static Task<bool> UpdateUserAsync(string key, User user)
        {
            try
            {
                await firebase
                    .Child("Users")
                    .Child(key)
                    .PutAsync(user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static async Task<User> GetUserAsync(string key)
        {
            try
            {
                return await firebase
                    .Child("Users")
                    .Child(key)
                    .OnceSingleAsync<User>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async static Task<bool> DeleteItemAsync(string key)
        {
            //TODO
            //IRÁ SERVIR PARA DELETAR ITENS
            try
            {
                await firebase
                    .Child("Itens")
                    .Child(key)
                    .DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
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
            try
            {
                return (await firebase
                .Child("Users")
                .OnceAsync<User>()).Select(item => new User
                {
                    Key = item.Object.Key,
                    Id = item.Object.Id,
                    Email = item.Object.Email,
                    Sobre = item.Object.Sobre,
                    Nome = item.Object.Nome,
                    Buffer = item.Object.Buffer,
                    DisplayUserInMap = item.Object.DisplayUserInMap,
                    Latitude = item.Object.Latitude,
                    Longitude = item.Object.Longitude
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<User>> GetNearUsers()
        {
            try
            {
                return (await firebase
                .Child("Users")
                .OnceAsync<User>()).Select(item => new User
                {
                    Key = item.Object.Key,
                    Sobre = item.Object.Sobre,
                    Nome = item.Object.Nome,
                    Buffer = item.Object.Buffer,
                    DisplayUserInMap = item.Object.DisplayUserInMap,
                    Latitude = item.Object.Latitude,
                    Longitude = item.Object.Longitude
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
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
        /*public static async Task UpdateUser(User user)
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
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                db.Update(user);
            }
        }*/
    }
}
