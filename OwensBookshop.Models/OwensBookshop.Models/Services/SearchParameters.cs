using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class SearchParameters
    {
        public string BookTitle { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Authors { get; set; }

        public List<string> Genres { get; set; }
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public int GenreID { get; set; }

    }
}
