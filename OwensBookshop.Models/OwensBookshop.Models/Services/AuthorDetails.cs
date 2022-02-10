using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class AuthorDetails
    {
        public int AuthorID { get; set; }
        public Author Author { get; set; }
        public List<Book> ListOfBooks { get; set; }

    }
}
