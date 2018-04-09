using System.Collections.Generic;

namespace Bookstore.Interface
{   
    public interface IBookstore
    {
        IEnumerable<Book> GetBooks();

        IEnumerable<Book> GetBookAsync(string searchString, string searchOn);

        void AddBook(Book newBook);

        void UpdateBook(string title, Book updatedBook);

        void DeleteBook(Book delBook);

        void UpdateBooks(IEnumerable<Book> updatedBooks);
        
    }
}
