using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OwensBookShop.Models.Services;

namespace OwensBookShop.Website.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            var searchPage = createBookDetailsSearchPage(new SearchParameters());

            return View(searchPage);
        }

        [HttpPost]
        public ActionResult Index(SearchParameters searchParameters)
        {
            var searchPage = createBookDetailsSearchPage(searchParameters);

            return View(searchPage);
        }

        private SearchPage<List<BookDetails>> createBookDetailsSearchPage(SearchParameters searchParameters)
        {
            SearchPage<List<BookDetails>> searchPage = new SearchPage<List<BookDetails>>();
            searchPage.SearchParameters = searchParameters;
            searchPage.SearchResults = searchBooks(searchParameters);

            return searchPage;
        }


        private SearchResults<List<BookDetails>> searchBooks(SearchParameters searchParameters)
        {
            var searchService = new Services.ShopDatabaseService();
            
            return searchService.SearchBooks(searchParameters);
        }

    }
}