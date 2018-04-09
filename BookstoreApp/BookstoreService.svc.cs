using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Bookstore.Interface;
using Newtonsoft.Json;


namespace BookstoreApp
{

    // WCF - Server repository to interact the server side
    public class BookstoreService : IBookstoreService
    {
        public BookstoreService()
        {
        }

        public IEnumerable<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            using (var webClient = new WebClient())
            {
                // 1- To load information from URL direct
                //string json = webClient.DownloadString(@"https://raw.githubusercontent.com/contribe/contribe/dev/arbetsprov-net/books.json");

                // 2- or to read from file which locate in BookstoreApp\bin directory 
                string path = AppDomain.CurrentDomain.BaseDirectory + "/bin/";
                string json = File.ReadAllText(path + @"books.json");

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);

                foreach (Book book in obj.Books)
                {
                    books.Add(new Book()
                    {
                        //Title = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(book.Title)),// we need this converting if read file from http (1-)
                        Title=book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        InStock = book.InStock
                    });
                }

                return books;
            }
            
        }

        public IEnumerable<Book> GetBookAsync(string searchString, string searchOn)
        {
            List<Book> books = new List<Book>();
            using (var webClient = new WebClient())
            {
                // 1- kan take load information from URL direct
                //string json = webClient.DownloadString(@"https://raw.githubusercontent.com/contribe/contribe/dev/arbetsprov-net/books.json");

                // 2- or to read from file which locate in BookstoreApp\bin directory 
                string path = AppDomain.CurrentDomain.BaseDirectory + "/bin/";
                string json = File.ReadAllText(path + @"books.json");

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(json);

                foreach (Book book in obj.Books)
                {
                    books.Add(new Book()
                    {   //Title = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(book.Title)),
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    InStock = book.InStock
                    });
                }

                if (searchOn == "title")
                    {return books.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));}
                else
                    { return books.Where(s => s.Author.ToLower().Contains(searchString.ToLower())); }
            }
        }

        public void AddBook(Book newBook)
        {
            List<Book> books = new List<Book>();
            books = GetBooks().ToList();
            books.Add(new Book()
            {
                Title = newBook.Title,
                Author=newBook.Author,
                Price=newBook.Price,
                InStock=newBook.InStock
            });

            JsonFile jsonData = new JsonFile();
            jsonData.Id = 1;
            jsonData.Name = "books";
            jsonData.Books = new List<Book>();
            jsonData.Books = books.ToList();

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/bin/";
            File.WriteAllText(path + @"books.json", json);
        }

        public void DeleteBook(Book delBook)
        {
            List<Book> listToSave = new List<Book>();

            var books = GetBooks().ToList();
            foreach(var book in books)
            { if (!((book.Title).Equals(delBook.Title) && (book.Author).Equals(delBook.Author)))
                {
                    listToSave.Add(new Book()
                    {   //Title = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(book.Title)),
                        Title = book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        InStock = book.InStock
                    });
                }
            }

            JsonFile jsonData = new JsonFile();
            jsonData.Id = 1;
            jsonData.Name = "books";
            jsonData.Books = new List<Book>();
            jsonData.Books = listToSave.ToList();

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/bin/";
            File.WriteAllText(path + @"books.json", json);

        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void UpdateBook(string title, Book updatedBook)
        {
            throw new NotImplementedException();
        }

        public void UpdateBooks(IEnumerable<Book> updatedBooks)
        {
            List<Book> listToSave = new List<Book>();
            var books = GetBooks().ToList();
            foreach (var book in books)
            {
                foreach (var updBook in updatedBooks)
                {
                    if (((book.Title).Equals(updBook.Title) && (book.Author).Equals(updBook.Author)))
                    {
                        book.InStock = updBook.InStock;
                    }
                }
                listToSave.Add(new Book()
                {   //Title = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(book.Title)),
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    InStock = book.InStock
                });
            }

            JsonFile jsonData = new JsonFile();
            jsonData.Id = 1;
            jsonData.Name = "books";
            jsonData.Books = new List<Book>();
            jsonData.Books = listToSave.ToList();

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/bin/";
            File.WriteAllText(path + @"books.json", json);
           
        }
    }
}
