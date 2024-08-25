using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Controllers
{
    internal class Product : Controller
    {
        public int Code { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public int Price { get; private set; }

        public Product(int code, string name, string unit, int price)
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
