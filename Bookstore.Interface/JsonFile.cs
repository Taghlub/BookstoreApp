using System.Collections.Generic;

namespace Bookstore.Interface
{   
    // Json schema used when write to Json file

    public class JsonFile
    {
        private int id;
        private string name;
        private List<Book> books;

        public JsonFile()
        {
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public List<Book> Books { get => books; set => books = value; }
    }
}
