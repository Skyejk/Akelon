using System;
using Task3.domain.controllers;

namespace Task3.controllers
{
    internal class Client : Controller
    {
        public string Code { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }

        public Client (string code, string organizationName, string address, string contactPerson)
        {
            Code = code;
            OrganizationName = organizationName;
            Address = address;
            ContactPerson = contactPerson;
        }
        public override void GetAll()
        {
            //
        }
    }
}
