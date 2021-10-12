using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


//CLASSE TEMPORÁRIA!!

namespace TCCApp.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Sobre { get; set; }
        public byte[] Buffer { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
