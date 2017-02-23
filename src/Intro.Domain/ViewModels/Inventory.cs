using System;
using System.Collections;
using System.Collections.Generic;
using Intro.Domain.PersistentModels;

namespace Intro.Domain.ViewModels
{
    public class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(IEnumerable<Item> items)
        {
            this.Items = items;
        }

        public IEnumerable<Item> Items { get; set; }
    } 
}
