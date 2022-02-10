using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OwensBookShop.Models.Services;

namespace OwensBookShop.Services
{
    public interface IShopService
    {
        SearchResults<List<BookDetails>> SearchBooks(SearchParameters searchParameters);
        ServiceResponse<BookDetails> SaveBookDetails(BookDetails bookDetails);
        ServiceResponse<Author> SaveAuthor(Author author);
        ServiceResponse<Genre> SaveGenre(Genre genre);
        ServiceResponse<List<Author>> GetAuthors(SearchParameters searchParameters);
        ServiceResponse<Author> GetAuthor(SearchParameters searchParameters);
        ServiceResponse<List<Genre>> GetGenres(SearchParameters searchParameters);
        ServiceResponse<Genre> GetGenre(SearchParameters searchParameters);

        ServiceResponse<bool> SaveBookAuthors(List<BookAuthor> authors);
        ServiceResponse<bool> SaveBookGenres(List<BookGenre> genres);

        ServiceResponse<AuthorDetails> GetAuthorDetails(int authorID);

        ServiceResponse<AuthorDetails> SaveAuthorDetails(AuthorDetails authorDetails);
    }
}
