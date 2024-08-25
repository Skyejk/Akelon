using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Models
{
    internal class DataHandler : IDisposable
    {
        private string FilePath { get; set; }

        public DataHandler(string filePath) 
        {
            FilePath = filePath;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
