using System;
using System.Collections.Generic;
using System.Text;
using TCCApp.Model;
using TCCApp.Services;
using Xamarin.Forms;

//Dependency faz a classe ItemData funcionar como uma variavel global em todo o projeto
//Posso referenciar ela através da interface IItemService
[assembly: Dependency(typeof(ItemData))]
namespace TCCApp.Model
{
    //CLASSE TEMPORÁRIA
    public class ItemData : IItemService
    {
        public IList<Item> Items { get; set; }

        public ItemData()
        {
            Items = new List<Item>();

            Items.Add(new Item
            {
                Nome = "Camisa",
                Cor = Color.FromHex("#BDF5F5") ,
                ImageUrl = "camisaIcon.png",
                Descricao = "test",
                Quantidade = 5
            });

           Items.Add(new Item
            {
                Nome = "Shorts",
                Cor = Color.FromHex("#F5BDEF"),
                ImageUrl = "shorts.png",
                Descricao = "",
                Quantidade = 6
           });

            Items.Add(new Item
            {
                Nome = "Garrafa",
                Cor = Color.FromHex("#EDF5BD"),
                ImageUrl = "garrafaIcon.png",
                Descricao = "",
                Quantidade = 2
            });
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }
    }
}
