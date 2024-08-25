using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Controllers
{
    internal class Client : Controller
    {
        public int Code { get; private set; }
        public string OrganizationName { get; private set; }
        public string Address { get; private set; }
        public string ContactPerson { get; private set; }

        public Client(int code, string organizationName, string address, string contactPerson)
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
