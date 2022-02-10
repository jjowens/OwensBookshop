using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class Book
    {
        public int BookID { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public int PublishYear { get; set; }
        public decimal Price { get; set; }

    }
}
