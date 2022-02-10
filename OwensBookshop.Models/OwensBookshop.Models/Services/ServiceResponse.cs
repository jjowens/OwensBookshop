using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public int ID { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
