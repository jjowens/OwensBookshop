using OwensBookShop.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OwensBookShop.Website.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        private SearchResults<List<BookDetails>> searchBooks(SearchParameters searchParameters)
        {
            var searchService = new Services.ShopDatabaseService();


            return searchService.SearchBooks(searchParameters);
        }

        private SearchResults<List<Author>> searchAuthors(SearchParameters searchParameters)
        {
            var searchService = new Services.ShopDatabaseService();


            return searchService.SearchAuthors(searchParameters);
        }

        public ActionResult Books()
        {
            var searchPage = createBookSearchPage(new SearchParameters());

            return View(searchPage);
        }

        [HttpPost]
        public ActionResult Books(SearchParameters searchParameters)
        {
            var searchPage = createBookSearchPage(new SearchParameters());

            return View(searchPage);
        }

        private SearchPage<List<BookDetails>> createBookSearchPage(SearchParameters searchParameters)
        {
            SearchPage<List<BookDetails>> searchPage = new SearchPage<List<BookDetails>>();
            searchPage.SearchParameters = searchParameters;
            searchPage.SearchResults = searchBooks(searchParameters);

            return searchPage;
        }

        private SearchPage<List<Author>> createAuthorSearchPage(SearchParameters searchParameters)
        {
            SearchPage<List<Author>> searchPage = new SearchPage<List<Author>>();
            searchPage.SearchParameters = searchParameters;
            searchPage.SearchResults = searchAuthors(searchParameters);

            return searchPage;
        }

        public ActionResult Authors()
        {
            var searchPage = createAuthorSearchPage(new SearchParameters());

            return View(searchPage);
        }

        [HttpPost]
        public ActionResult Authors(SearchParameters searchParameters)
        {
            var searchPage = createAuthorSearchPage(searchParameters);

            return View();
        }
    }
}