using Firebase.Database;
using Firebase.Database.Query;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using Xamarin.Forms;

namespace TCCApp.Services
{
    public class DatabaseService
    {
        public static FirebaseClient firebase = new FirebaseClient("https://dbteste-cbb09-default-rtdb.firebaseio.com/");

        public async static Task<bool> AddMessage(OutboundMessage message, string groupKey)
        {
            try
            {
                message.Key = await GetChatKey(groupKey);
                await UpdateMessageAsync(message.Key, groupKey, message);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async static Task<bool> UpdateMessageAsync(string key, string groupKey, OutboundMessage message)
        {
            try
            {
                await firebase
                    .Child("Chat")
                    .Child(groupKey)
                    .Child(key)
                    .PutAsync(message);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async static Task<string> GetConversaKey(string table)
        {
            //Esse método cria um documento vazio e retorna uma chave
            var doc = await firebase
               .Child(table)
               .PostAsync(new Message { Author = "temp" });
            return doc.Key;
        }
        public async static Task<string> GetNewChatGroupKey()
        {
            //Esse método cria um documento vazio e retorna uma chave
            var doc = await firebase
               .Child("Chat")
                  .PostAsync(new OutboundMessage());
            return doc.Key;
        }
        public async static Task<string> GetChatKey(string value)
        {
            //Esse método cria um documento vazio e retorna uma chave
            var doc = await firebase
               .Child("Chat")
               .Child(value)
                  .PostAsync(new OutboundMessage());
            return doc.Key;
        }
        public async static Task<List<OutboundMessage>> GetMessages(string groupKey)
        {
            try
            {
                return (await firebase
                .Child("Chat")
                .Child(groupKey)
                .OnceAsync<OutboundMessage>()).Select(item => new OutboundMessage
                {
                    Author = item.Object.Author,
                    Content = item.Object.Content
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async static Task<bool> AddNotificacao(Notification notification)
        {
            try
            {
                notification.Key = await GetConversaKey("Notificacao");
                await UpdateNotificacao(notification.Key, notification);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async static Task<bool> UpdateNotificacao(string key, Notification notification)
        {
            try
            {
                await firebase
                    .Child("Notificacao")
                    .Child(key)
                    .PutAsync(notification);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public async static Task<bool> AddUserAsync(User user)
        {
            try
            {
                user.Key = await GetUserKeyAsync();
                await UpdateUserAsync(user.Key, user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async static Task<string> GetUserKeyAsync()
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
                    Longitude = item.Object.Longitude,
                    Senha = item.Object.Senha
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
        

        /*public static async Task AddUser(User user)
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

        }*/
        /*public static async Task<User> GetUser(int Id)
        {
            var allUsers = await GetUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(u => u.Id == Id).FirstOrDefault();
        }*/
        /*public static async Task<User> GetUser(User user)
        {
            var allUsers = await GetUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(u => u.Id == user.Id).FirstOrDefault();
        }*/
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

        public static async Task<User> GetUser(string email)
        {
            var allUsers = await GetUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(a => a.Email == email).FirstOrDefault();
        }
        public static bool IsFormValid(object model, Page page)
        {
            HideValidationFields(model, page);
            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, context, errors, true);
            if (!isValid)
            {
                ShowValidationFields(errors, model, page);
            }
            return errors.Count() == 0;
        }
        private static void HideValidationFields
            (object model, Page page, string validationLabelSuffix = "Error")
        {
            if (model == null) { return; }
            var properties = GetValidatablePropertyNames(model);
            foreach (var propertyName in properties)
            {
                var errorControlName =
                $"{propertyName.Replace(".", "_")}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);
                if (control != null)
                {
                    control.IsVisible = false;
                }
            }
        }
        private static void ShowValidationFields(List<ValidationResult> errors, object model, Page page, string validationLabelSuffix = "Error")
        {
            const string senhaRequired = "The Senha field is required.";
            const string emailRequired = "The Email field is required.";
            const string emailValid = "The Email field is not a valid e-mail address.";
            const string senhaObrigatorio = "O campo Senha é obrigatório.";
            const string emailObrigatorio = "O campo Email é obrigatório.";
            const string emailValido = "Por favor, entre com um email válido.";

            if (model == null) { return; }
            foreach (var error in errors)
            {
                var memberName = $"{model.GetType().Name}_{error.MemberNames.FirstOrDefault()}";
                memberName = memberName.Replace(".", "_");
                var errorControlName = $"{memberName}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);
                if (control != null)
                {
                    switch (error.ErrorMessage)
                    {
                        case senhaRequired:
                            control.Text = senhaObrigatorio;
                            break;
                        case emailRequired:
                            control.Text = emailObrigatorio;
                            break;
                        case emailValid:
                            control.Text = emailValido;
                            break;
                        default:
                            control.Text = $"{error.ErrorMessage}{Environment.NewLine}";
                            break;
                    }
                    control.IsVisible = true;
                }
            }
        }
        private static IEnumerable<string> GetValidatablePropertyNames(object model)
        {
            var validatableProperties = new List<string>();
            var properties = GetValidatableProperties(model);
            foreach (var propertyInfo in properties)
            {
                var errorControlName = $"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}";
                validatableProperties.Add(errorControlName);
            }
            return validatableProperties;
        }
        private static List<PropertyInfo> GetValidatableProperties(object model)
        {
            var properties = model.GetType().GetProperties().Where(prop => prop.CanRead
                && prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any()
                && prop.GetIndexParameters().Length == 0).ToList();
            return properties;
        }
    }
}
