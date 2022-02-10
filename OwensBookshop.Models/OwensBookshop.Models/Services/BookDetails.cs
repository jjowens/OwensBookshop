using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class BookDetails
    {
        public Book Book { get; set; }
        public List<Author> Authors { get; set; }
        public List<Genre> Genres { get; set; }

    }
}
