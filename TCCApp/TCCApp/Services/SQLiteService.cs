using Plugin.Geolocator.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCCApp.Model;

namespace TCCApp.Services
{
    public class SQLiteService
    {
        public static void AddUser(User user)
        {
            using(var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                db.Insert(user);
            }
        }
        public static void UpdateUser(User user)
        {
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                db.Update(user);
            }
        }
        public static List<User> GetUsers()
        {
            List<User> users;
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                users = db.Table<User>().ToList();
            }
            return users;
        }
        /*public static User GetUser(string userId)
        {
            User user;
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                var users = db.Table<User>().ToList();
                user = users.Where(u => u.Id == userId).FirstOrDefault();
            }
            return user;
        }*/
        public static User UpdatePosition(User user, Position position)
        {
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                user.Latitude = position.Latitude;
                user.Longitude = position.Longitude;
                db.CreateTable<User>();
                db.Update(App.user);
            }
            return user;
        }
        public static IEnumerable<User> GetNearUsers(double raio)
        {
            IEnumerable<User> users;
            using (var db = new SQLiteConnection(App.DatabasePath))
            {
                db.CreateTable<User>();
                users = db.Table<User>().ToList().Where(u => u.Id != App.user.Id &&
                (DistanceService.CompareDistance(App.user.Latitude, App.user.Longitude, u.Latitude, u.Longitude) <= (raio / 1000)));
            }
            return users;
        }
    }
}
