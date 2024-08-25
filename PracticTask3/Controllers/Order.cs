using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Controllers
{
    internal class Order : Controller
    {
        public int Code { get; private set; }
        public int ProductCode { get; private set; }
        public int ClientCode { get; private set; }
        public int RequestNumber { get; private set; }
        public int Quantity { get; private set; }
        public DateOnly OrderDate { get; private set; }

        public Product? Product { get; set; }
        public Client? Client { get; set; }

        public Order(int code, int productCode, int clientCode, int requestNumber, int quantity, DateOnly orderDate)
        {
            Code = code;
            ProductCode = productCode;
            ClientCode = clientCode;
            RequestNumber = requestNumber;
            Quantity = quantity;
            OrderDate = orderDate;
        }

        public override void GetAll()
        {
            //
        }
    }
}
