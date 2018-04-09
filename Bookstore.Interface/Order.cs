using System;
using System.Collections.Generic;

namespace Bookstore.Interface
{
    public class Order
    {
        private string orderNo;
        private DateTime orderDate;
        private string custName;
        private string custEmail;
        private List<Book> books;

        public Order()
        {
        }

        public string OrderNo { get => orderNo; set => orderNo = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public string CustName { get => custName; set => custName = value; }
        public string CustEmail { get => custEmail; set => custEmail = value; }
        public List<Book> Books { get => books; set => books = value; }
    }
}
