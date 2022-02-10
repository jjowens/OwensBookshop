using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OwensBookShop.Models.Services;


namespace OwensBookShop.Website.helpers
{
    public static class CustomHelper
    {
        public static string DisplayAuthors(List<Author> authors)
        {
            string results = string.Empty;

            if (authors != null)
            {
                results = string.Join(", ", authors.OrderBy(item => item.LastName).Select(x => string.Format("{0}", x.FullName)).ToArray());
            }

            return results;
        }

        public static string DisplayAuthorLinks(List<Author> authors)
        {
            string results = string.Empty;

            if (authors != null)
            {
                results = string.Join(", ", authors.OrderBy(item => item.LastName).Select(x => string.Format("{0}", x.FullName)).ToArray());
            }

            return results;
        }


    }
}