using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using Xamarin.Forms;

namespace TCCApp.Helpers
{
    class UsuarioHelper
    {
        static FirebaseClient firebase = new FirebaseClient("https://xamarinfirebase-3c01a-default-rtdb.firebaseio.com/");

        public static async Task<List<Usuario>> GetAllUsers()
        {

            return (await firebase
              .Child("Usuarios")
              .OnceAsync<Usuario>()).Select(item => new Usuario
              {
                  Email = item.Object.Email,
                  Senha = item.Object.Senha
              }).ToList();
        }

        public static async Task<bool> AddUser(string email, string senha)
        {
            try
            {
                await firebase
              .Child("Usuarios")
              .PostAsync(new Usuario() { Email = email, Senha = senha });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        public static async Task<Usuario> GetUser(string email)
        {
            var allUsers = await GetAllUsers();
            await firebase
              .Child("Usuarios")
              .OnceAsync<Usuario>();
            return allUsers.Where(a => a.Email == email).FirstOrDefault();
        }

        public async Task UpdateUser(string email, string senha)
        {
            var toUpdateUser = (await firebase
              .Child("Usuarios")
              .OnceAsync<Usuario>()).Where(a => a.Object.Email == email).FirstOrDefault();

            await firebase
              .Child("Usuarios")
              .Child(toUpdateUser.Key)
              .PutAsync(new Usuario() { Email = email, Senha = senha });
        }

        public async Task DeleteUser(string email)
        {
            var toDeleteUser = (await firebase
              .Child("Usuarios")
              .OnceAsync<Usuario>()).Where(a => a.Object.Email == email).FirstOrDefault();
            await firebase.Child("Usuarios").Child(toDeleteUser.Key).DeleteAsync();

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
        private static void ShowValidationFields (List<ValidationResult> errors, object model, Page page, string validationLabelSuffix = "Error")
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
