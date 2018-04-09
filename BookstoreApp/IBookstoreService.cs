using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Bookstore.Interface;

namespace BookstoreApp
{
    // 
    [ServiceContract]
    public interface IBookstoreService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // The service operations here

        [OperationContract]
        IEnumerable<Book> GetBooks();

        [OperationContract]
        IEnumerable<Book> GetBookAsync(string searchString, string searchOn );

        [OperationContract]
        void AddBook(Book newBook);

        [OperationContract]
        void UpdateBook(string title, Book updatedBook);

        [OperationContract]
        void DeleteBook(Book delBook);

        [OperationContract]
        void UpdateBooks(IEnumerable<Book> updatedBooks);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
