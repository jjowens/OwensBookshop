using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class SearchPage<T>
    {
        public SearchParameters SearchParameters { get; set; }
        public SearchResults<T> SearchResults { get; set; }

    }
}
