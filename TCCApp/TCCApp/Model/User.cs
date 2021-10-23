using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


//CLASSE TEMPORÁRIA!!

namespace TCCApp.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Nome { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Sobre { get; set; }
        public byte[] Buffer { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool DisplayUserInMap { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
