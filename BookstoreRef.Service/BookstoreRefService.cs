using System;
using System.Collections.Generic;
using Bookstore.Interface;
using BookstoreRef.Service.MyService;
using System.Linq;


namespace BookstoreRef.Service
{
    // Service repository to interact the Client side

    public class BookstoreRefService : IBookstore
    {
        BookstoreServiceClient serviceProxy = new BookstoreServiceClient();

        public IEnumerable<Book> GetBooks()
        {
            return serviceProxy.GetBooks();
        }

        public IEnumerable<Book> GetBookAsync(string searchString, string searchOn)
        {
            return serviceProxy.GetBookAsync(searchString, searchOn);
        }

        public void AddBook(Book newBook)
        {
            serviceProxy.AddBook(newBook);
        }

        public void DeleteBook(Book delBook)
        {    
            serviceProxy.DeleteBook(delBook);
        }

        public void UpdateBook(string title, Book updatedBook)
        {
            throw new NotImplementedException();
        }

        public void UpdateBooks(IEnumerable<Book> updatedBooks)
        {
            serviceProxy.UpdateBooks(updatedBooks.ToList());
        }
 
    }
}
