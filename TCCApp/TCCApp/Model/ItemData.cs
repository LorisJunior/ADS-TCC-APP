using System;
using System.Collections.Generic;
using System.Text;

namespace TCCApp.Model
{
    //CLASSE TEMPORÁRIA
    public class ItemData
    {
        public IList<Item> Items { get; set; }


        public ItemData()
        {
            Items = new List<Item>();

            Items.Add(new Item
            {
                Nome = "Camisa",
                Cor = "#BDF5F5",
                ImageUrl = "delete.png",
                Descricao = ""
            });

           Items.Add(new Item
            {
                Nome = "Shots",
                Cor = "#F5BDEF",
                ImageUrl = "chat.png",
                Descricao = ""
            });

            Items.Add(new Item
            {
                Nome = "Garrafa",
                Cor = "#EDF5BD",
                ImageUrl = "edit.png",
                Descricao = ""
            });
        }
    }
}
