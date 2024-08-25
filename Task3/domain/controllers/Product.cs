using System;
using Task3.domain.controllers;

namespace Task3.controllers
{
    internal class Product : Controller
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Price { get; set; }

        public Product(string code, string name, string unit, string price)
        {
            Code = code;
            Name = name;
            Unit = unit;
            Price = price;
        }
        public override void GetAll()
        {
            //
        }
    }
}
