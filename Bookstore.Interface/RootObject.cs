using System.Collections.Generic;


namespace Bookstore.Interface
{
    // used on Deserialize JSON object

    public class RootObject
    {
        private List<Book> books;

        public List<Book> Books { get => books; set => books = value; }
       
    }
}
