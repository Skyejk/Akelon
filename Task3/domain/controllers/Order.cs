using System;
using Task3.domain.controllers;

namespace Task3.controllers
{
    internal class Order : Controller
    {
        public string Code { get; set; }
        public string ProductCode { get; set; }
        public string ClientCode { get; set; }
        public string RequestNumber { get; set; }
        public string Quantity { get; set; }
        public string OrderDate { get; set; }

        public Order (string code, string productCode, string clientCode, string requestNumber, string quantity, string orderDate)
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
