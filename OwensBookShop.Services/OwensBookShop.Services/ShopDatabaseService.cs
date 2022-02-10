using OwensBookShop.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwensBookShop.Services
{
    public class ShopDatabaseService : IShopService
    {
        public ServiceResponse<Author> GetAuthor(SearchParameters searchParameters)
        {
            ServiceResponse<Author> serviceResponse = new ServiceResponse<Author>();

            using(var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Authors.AsQueryable();

                if (searchParameters.AuthorID > 0)
                {
                    query = query.Where(item => item.AuthorID == searchParameters.AuthorID);
                }

                var data = (from item in query
                            select new Author()
                            {
                                AuthorID = item.AuthorID,
                                FirstName = item.Firstname,
                                LastName = item.LastName,
                                FullName = item.FullName
                            }).FirstOrDefault();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<List<Author>> GetAuthors(SearchParameters searchParameters)
        {
            ServiceResponse<List<Author>> serviceResponse = new ServiceResponse<List<Author>> ();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Authors.AsQueryable();


                var data = (from item in query
                            select new Author()
                            {
                                AuthorID = item.AuthorID,
                                FirstName = item.Firstname,
                                LastName = item.LastName,
                                FullName = item.FullName
                            }).ToList();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<Genre> GetGenre(SearchParameters searchParameters)
        {
            ServiceResponse<Genre> serviceResponse = new ServiceResponse<Genre>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Genres.AsQueryable();

                if (searchParameters.GenreID > 0)
                {
                    query = query.Where(item => item.GenreID == searchParameters.GenreID);
                }


                var data = (from item in query
                            select new Genre()
                            {
                                GenreID = item.GenreID,
                                GenreName = item.GenreName
                            }).FirstOrDefault();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<List<Genre>> GetGenres(SearchParameters searchParameters)
        {
            ServiceResponse<List<Genre>> serviceResponse = new ServiceResponse<List<Genre>>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Genres.AsQueryable();

                var data = (from item in query
                            select new Genre()
                            {
                                GenreID = item.GenreID,
                                GenreName = item.GenreName
                            }).ToList();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<Author> SaveAuthor(Author author)
        {
            ServiceResponse<Author> serviceResponse = new ServiceResponse<Author>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Authors.AsQueryable();

                if (author.AuthorID > 0)
                {
                    query = query.Where(item => item.AuthorID == author.AuthorID);
                } else
                {
                    query = query.Where(item => item.FullName.ToLower().Trim() == author.FullName.ToLower().Trim());
                }

                var authorObj = query.FirstOrDefault();

                // CREATE RECORD IF AUTHOR DOES NOT EXISTS. OTHERWISE, RETURN CURRENT RECORD
                if (authorObj == null)
                {
                    authorObj = new Engine.Author()
                    {
                        Firstname = author.FirstName,
                        LastName = author.LastName,
                        FullName = author.FullName
                    };

                    dbContext.Authors.InsertOnSubmit(authorObj);

                    try
                    {
                        dbContext.SubmitChanges();
                    } catch (Exception)
                    {
                        throw;
                    }

                }

                var data = new Models.Services.Author()
                {
                    AuthorID = authorObj.AuthorID,
                    FirstName = authorObj.Firstname,
                    LastName = authorObj.LastName
                };

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<bool> SaveBookAuthors(List<BookAuthor> authors)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var bookID = authors.FirstOrDefault().BookID;

                var listOfCurrentAuthors = dbContext.BookAuthors.Where(item => item.BookID == bookID).ToList();

                var newAuthors = new List<Engine.BookAuthor>();

                if (listOfCurrentAuthors.Any())
                {
                    foreach(var authorObj in listOfCurrentAuthors)
                    {
                        var existObj = listOfCurrentAuthors.Where(item => item.AuthorID == authorObj.AuthorID).FirstOrDefault();

                        if (existObj == null)
                        {
                            newAuthors.Add(new Engine.BookAuthor()
                            {
                                BookID = bookID,
                                AuthorID = authorObj.AuthorID
                            });
                        }
                    }
                }

                // CREATE RECORD IF AUTHOR DOES NOT EXISTS. OTHERWISE, RETURN CURRENT RECORD
                if (newAuthors.Any())
                {

                    dbContext.BookAuthors.InsertAllOnSubmit(newAuthors);

                    try
                    {
                        dbContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                serviceResponse.Data = true;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<BookDetails> SaveBookDetails(BookDetails bookDetails)
        {
            ServiceResponse<BookDetails> serviceResponse = new ServiceResponse<BookDetails>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                List<Author> authors = new List<Author>();
                List<Genre> genres = new List<Genre>();

                var query = dbContext.Books.AsQueryable();

                query = query.Where(item => item.BookID == bookDetails.Book.BookID);

                var bookObj = query.FirstOrDefault();

                if (bookObj == null)
                {
                    bookObj = new Engine.Book()
                    {
                        Title = bookDetails.Book.BookTitle,
                        Price = bookDetails.Book.Price,
                        PublishYear = bookDetails.Book.PublishYear,
                        ISBN = bookDetails.Book.ISBN

                    };

                    try
                    {
                        dbContext.Books.InsertOnSubmit(bookObj);
                    } 
                        catch(Exception)
                    {

                    }

                }

                // SAVE BOOK AUTHORS AND GENRES
                if (bookDetails.Authors.Any())
                {
                    ServiceResponse<Author> authorService = new ServiceResponse<Author>();
                    foreach (var authorObj in bookDetails.Authors)
                    {
                        authorService = SaveAuthor(authorObj);
                        authors.Add(authorService.Data);
                    }

                    var bookAuthors = (from item in authors
                                       select new BookAuthor()
                                       {
                                           BookID = bookObj.BookID,
                                           AuthorID = item.AuthorID
                                       }).ToList();

                    SaveBookAuthors(bookAuthors);
                }

                if (bookDetails.Genres.Any())
                {
                    ServiceResponse<Genre> genreService = new ServiceResponse<Genre>();
                    foreach (var genreObj in bookDetails.Genres)
                    {
                        genreService = SaveGenre(genreObj);
                        genres.Add(genreService.Data);
                    }

                    var bookGenres = (from item in genres
                                       select new BookGenre()
                                       {
                                           BookID = bookObj.BookID,
                                           GenreID = item.GenreID
                                       }).ToList();

                    SaveBookGenres(bookGenres);
                }

                var data = (from book in dbContext.Books
                            where book.BookID == bookObj.BookID
                            select new BookDetails()
                            {
                                Book = new Book()
                                {
                                    BookID = book.BookID,
                                    BookTitle = book.Title,
                                    Price = book.Price.HasValue ? book.Price.Value : 0,
                                    PublishYear = book.PublishYear.HasValue ? book.PublishYear.Value : 0
                                },
                                Authors = (from author in dbContext.Authors
                                           join bookAuthor in dbContext.BookAuthors on author.AuthorID equals bookAuthor.AuthorID
                                           where bookAuthor.BookID == bookObj.BookID
                                           select new Author()
                                           {
                                               AuthorID = author.AuthorID,
                                               FirstName = author.Firstname,
                                               LastName = author.LastName,
                                               FullName = author.FullName
                                           }).ToList(),
                                Genres = (from genre in dbContext.Genres
                                           join bookGenre in dbContext.BookGenres on genre.GenreID equals bookGenre.GenreID
                                           where bookGenre.BookID == bookObj.BookID
                                           select new Genre()
                                           {
                                               GenreID = genre.GenreID,
                                               GenreName = genre.GenreName
                                           }).ToList()
                            }).FirstOrDefault();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<bool> SaveBookGenres(List<BookGenre> genres)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var bookID = genres.FirstOrDefault().BookID;

                var listOfCurrentGenres = dbContext.BookGenres.Where(item => item.BookID == bookID).ToList();

                var newGenres = new List<Engine.BookGenre>();

                if (listOfCurrentGenres.Any())
                {
                    foreach (var genreObj in listOfCurrentGenres)
                    {
                        var existObj = listOfCurrentGenres.Where(item => item.GenreID == genreObj.GenreID).FirstOrDefault();

                        if (existObj == null)
                        {
                            newGenres.Add(new Engine.BookGenre()
                            {
                                BookID = bookID,
                                GenreID = genreObj.GenreID
                            });
                        }
                    }
                }

                // CREATE RECORD IF AUTHOR DOES NOT EXISTS. OTHERWISE, RETURN CURRENT RECORD
                if (newGenres.Any())
                {

                    dbContext.BookGenres.InsertAllOnSubmit(newGenres);

                    try
                    {
                        dbContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                serviceResponse.Data = true;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<Genre> SaveGenre(Genre genre)
        {
            ServiceResponse<Genre> serviceResponse = new ServiceResponse<Genre>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = dbContext.Genres.AsQueryable();

                if (genre.GenreID > 0)
                {
                    query = query.Where(item => item.GenreID == genre.GenreID);
                }
                else
                {
                    query = query.Where(item => item.GenreName.ToLower().Trim() == genre.GenreName.ToLower().Trim());
                }

                var genreObj = query.FirstOrDefault();

                // CREATE RECORD IF GENRE DOES NOT EXISTS. OTHERWISE, RETURN CURRENT RECORD
                if (genreObj == null)
                {
                    genreObj = new Engine.Genre()
                    {
                        GenreName = genre.GenreName,
                    };

                    dbContext.Genres.InsertOnSubmit(genreObj);

                    try
                    {
                        dbContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                var data = new Models.Services.Genre()
                {
                    GenreID = genre.GenreID,
                    GenreName = genre.GenreName
                };

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public SearchResults<List<BookDetails>> SearchBooks(SearchParameters searchParameters)
        {
            SearchResults<List<BookDetails>> serviceResponse = new SearchResults<List<BookDetails>>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = (from book in dbContext.Books
                             select new
                             {
                                 book,
                                 authors = (from author in dbContext.Authors
                                            join bookAuthor in dbContext.BookAuthors on author.AuthorID equals bookAuthor.AuthorID
                                            where bookAuthor.BookID == book.BookID
                                            select author),
                                 genres = (from genre in dbContext.Genres
                                           join bookGenre in dbContext.BookGenres on genre.GenreID equals bookGenre.GenreID
                                           where bookGenre.BookID == book.BookID
                                           select genre)
                             });


                if (searchParameters.BookID > 0)
                {
                    query = query.Where(item => item.book.BookID == searchParameters.BookID);
                }

                var data = (from item in query
                            select new BookDetails()
                            {
                                Book = new Book()
                                {
                                    BookID = item.book.BookID,
                                    BookTitle = item.book.Title,
                                    Price = item.book.Price.HasValue ? item.book.Price.Value : 0,
                                    PublishYear = item.book.PublishYear.HasValue ? item.book.PublishYear.Value : 0
                                },
                                Authors = (from authorItem in item.authors
                                           select new Author()
                                           {
                                               AuthorID = authorItem.AuthorID,
                                               FirstName = authorItem.Firstname,
                                               LastName = authorItem.LastName,
                                               FullName = authorItem.FullName
                                           }).ToList(),
                                Genres = (from genreItem in item.genres
                                          select new Genre()
                                          {
                                              GenreID = genreItem.GenreID,
                                              GenreName = genreItem.GenreName
                                          }).ToList()
                            }).ToList();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }

        public ServiceResponse<AuthorDetails> SaveAuthorDetails(AuthorDetails authorDetails)
        {
            ServiceResponse<AuthorDetails> serviceResponse = new ServiceResponse<AuthorDetails>();

            var authorService = SaveAuthor(authorDetails.Author);

            List<Author> currentAuthors = new List<Author>();
            currentAuthors.Add(authorService.Data);

            List<BookDetails> listOfBookDetails = (from item in authorDetails.ListOfBooks
                                                   select new BookDetails
                                                   {
                                                       Book = item,
                                                       Authors = currentAuthors,
                                                       Genres = null
                                                   }).ToList();

            List<BookDetails> currentBooks = new List<BookDetails>();

            foreach(var bookItem in listOfBookDetails)
            {
                var newBook = SaveBookDetails(bookItem);

                currentBooks.Add(newBook.Data);
            }

            AuthorDetails newAuthorDetails = new AuthorDetails();
            newAuthorDetails.Author = authorService.Data;
            newAuthorDetails.AuthorID = authorService.Data.AuthorID;
            newAuthorDetails.ListOfBooks = currentBooks.Select(item => new Book()
            {
                BookID = item.Book.BookID,
                BookTitle = item.Book.BookTitle,
                ISBN = item.Book.ISBN,
                Price = item.Book.Price,
                PublishYear = item.Book.PublishYear
            }).ToList();

            serviceResponse.Data = newAuthorDetails;

            return serviceResponse;
        }

        public ServiceResponse<AuthorDetails> GetAuthorDetails(int authorID)
        {
            ServiceResponse<AuthorDetails> serviceResponse = new ServiceResponse<AuthorDetails>();

            using(var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var authorDetails = (from authorObj in dbContext.Authors
                                     where authorObj.AuthorID == authorID
                                     select new AuthorDetails()
                                     {
                                         AuthorID = authorObj.AuthorID,
                                         Author = new Author()
                                         {
                                             AuthorID = authorObj.AuthorID,
                                             FirstName = authorObj.Firstname,
                                             LastName = authorObj.LastName,
                                             FullName = authorObj.FullName,
                                         },
                                         ListOfBooks = (from bookObj in dbContext.Books
                                                        join bookAuthorObj in dbContext.BookAuthors on bookObj.BookID equals bookAuthorObj.BookID
                                                        where bookAuthorObj.AuthorID == authorID
                                                        select new Book()
                                                        {
                                                            BookID = bookObj.BookID,
                                                            BookTitle = bookObj.Title,
                                                            ISBN = bookObj.ISBN,
                                                            Price = bookObj.Price ?? 0,
                                                        }).ToList()
                                     }).FirstOrDefault();

                serviceResponse.Data = authorDetails;
                serviceResponse.ID = authorDetails.AuthorID;
                serviceResponse.Success = true;

            }

            return serviceResponse;
        }

        public SearchResults<List<Author>> SearchAuthors(SearchParameters searchParameters)
        {
            SearchResults<List<Author>> serviceResponse = new SearchResults<List<Author>>();

            using (var dbContext = new Engine.OwensBookshopDBDataContext())
            {
                var query = (from authorObj in dbContext.Authors
                             select authorObj);

                if (searchParameters.AuthorID > 0)
                {
                    query = (from item in query
                             where item.AuthorID == searchParameters.AuthorID
                             select item);
                }

                if (!string.IsNullOrEmpty(searchParameters.FullName))
                {
                    query = (from item in query
                             where item.FullName.ToLower() == searchParameters.FullName.ToLower()
                             select item);
                }

                var data = (from item in query
                            select new Author()
                            {
                                AuthorID = item.AuthorID,
                                FirstName = item.Firstname,
                                LastName = item.LastName,
                                FullName = item.FullName
                            }).ToList();

                serviceResponse.Data = data;
                serviceResponse.Success = true;
            }

            return serviceResponse;
        }
    }
}
