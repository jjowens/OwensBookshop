using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Models.Services
{
    public class SearchResults<T>
    {
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int ResultsPerPage { get; set; }
        public bool Success { get; set; }
        public bool Message { get; set; }
    }
}
