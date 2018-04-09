using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BookstoreRef.Service;
using Bookstore.Interface;
using System.Text;
using System.Linq;

namespace Bookstore.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// I write program here in code behind not in ViewModel (business logic layer)
    /// because I focus on the backend
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IBookstore bookstore = new BookstoreRefService();
            IEnumerable<Book> books = bookstore.GetBooks();
            Lst_Books.Items.Clear();
            foreach (var book in books)
              {Lst_Books.Items.Add(book);}

            Cmb_Search.Items.Add("Search on Title");
            Cmb_Search.Items.Add("Search on Author");
            Cmb_Search.SelectedIndex = 0;

            Txt_Author.Text = "";
            Txt_InStock.Text = "0";
            Txt_Price.Text = "0,0";
            Txt_Title.Text = "";

        }

        private void Txt_Serach_TextChanged(object sender, TextChangedEventArgs e)
        {
            IBookstore bookstore = new BookstoreRefService();
            IEnumerable<Book> books;

            if (Txt_Serach.Text.Length == 0)
            { books = bookstore.GetBooks(); }
            else
            {
                string searchOn = "";
                if (Cmb_Search.SelectedIndex == 0) { searchOn = "title"; } else { searchOn = "author"; }
                books = bookstore.GetBookAsync(Txt_Serach.Text, searchOn);
            }
            Lst_Books.Items.Clear();
            foreach (var book in books)
            { Lst_Books.Items.Add(book); }
        }

        private void Cmb_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Txt_Serach.Text = "";
        }

        #region Book events
        private void Btn_newBook_Click(object sender, RoutedEventArgs e)
        { Grp_Input.Visibility = Visibility.Visible;
            Btn_createOrder.IsEnabled = false;
        }

        private void Btn_delBook_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Book)
            {
                Book deleteme = (Book)cmd.DataContext;
                if (MessageBox.Show("Are you sure to delete ?", "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        IBookstore bookstore = new BookstoreRefService();
                        bookstore.DeleteBook(deleteme);
                        Lst_Books.Items.Remove(deleteme);
                    }
                    catch (Exception ex)
                    {
                        { MessageBox.Show(ex.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                    }
                }
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to add ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (Txt_Title.Text.Length == 0 || Txt_Author.Text.Length == 0)
                { MessageBox.Show("Check the inputs","", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                IBookstore bookstore = new BookstoreRefService();
                Book book = new Book();
                book.Title = Txt_Title.Text;
                book.Author = Txt_Author.Text;
                book.Price = Convert.ToDecimal( Txt_Price.Text.ToString());
                book.InStock = Convert.ToInt16(Txt_InStock.Text);
                bookstore.AddBook(book);
                Lst_Books.Items.Add(book);
            }
            cancel();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancel();
        }
        #endregion

        #region Order events
        private void Btn_createOrder_Click(object sender, RoutedEventArgs e)
        {
            Grp_Order.Visibility = Visibility.Visible;
            Txt_OrderDate.Text= DateTime.Now.ToString("yyyy-MM-dd");
            Btn_newBook.IsEnabled = false;
        }

        private void Btn_Add2List_Click(object sender, RoutedEventArgs e)
        {
            if (Lst_Books.SelectedIndex == -1) { MessageBox.Show("You should select a book ", "", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (Txt_Qty.Text.Length == 0) { MessageBox.Show("You should input the quentity ", "", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            try
            {
                int qty = Convert.ToInt16(Txt_Qty.Text);

                Book selectedItem = new Book();
                selectedItem = (Book)Lst_Books.SelectedItems[0];

                Book orderItem = new Book();
                orderItem.Title = selectedItem.Title;
                orderItem.Author = selectedItem.Author;
                orderItem.InStock = qty;
                orderItem.Price = selectedItem.Price * qty;
                Lst_Order.Items.Add(orderItem);

                decimal total = String.IsNullOrEmpty(Txt_TotalPrice.Text) ? 0 : Convert.ToDecimal(Txt_TotalPrice.Text);
                Txt_TotalPrice.Text = (total + selectedItem.Price * qty).ToString();

                Lst_Order.SelectedIndex = Lst_Order.Items.Count - 1;
                Lst_Order.ScrollIntoView(Lst_Order.Items[Lst_Order.SelectedIndex]);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Quentity must be a number ", "", MessageBoxButton.OK, MessageBoxImage.Error); return; ;
            }

        }

        private void Btn_SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Txt_OrderNo.Text)) { MessageBox.Show("You should input the Order number ", "", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (String.IsNullOrEmpty(Txt_CustName.Text)) { MessageBox.Show("You should input the Customer Name ", "", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            if (String.IsNullOrEmpty(Txt_CustEmail.Text)) { MessageBox.Show("You should input the Customer Email ", "", MessageBoxButton.OK, MessageBoxImage.Information); return; }

            if (MessageBox.Show("Are you sure to save Order ?", "Question",
                 MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    #region updating listbox lst_Book  

                    List<Book> lstBooksToUpd = new List<Book>();
                    StringBuilder custLetter = new StringBuilder();
                    custLetter.Append("Customer notification letter").AppendLine().AppendLine();

                    for (int i = 0; i < Lst_Order.Items.Count; i++)
                    {
                        Book orderItem = new Book();
                        Lst_Order.SelectedIndex = i;
                        orderItem = (Book)Lst_Order.SelectedItems[0];

                        for (int j = 0; j < Lst_Books.Items.Count; j++)
                        {
                            Book book = new Book();
                            Lst_Books.SelectedIndex = j;
                            book = (Book)Lst_Books.SelectedItems[0];

                            if ((book.Title).Equals(orderItem.Title) && (book.Author).Equals(orderItem.Author))
                            {
                                if (orderItem.InStock > book.InStock)
                                {
                                    custLetter.Append("- Required book: " + book.Title).AppendLine();
                                    custLetter.Append("  Auther: " + book.Author).AppendLine();
                                    custLetter.Append("  we have not enough quentity in stock ")
                                                        .AppendLine().AppendLine();
                                }
                                else
                                {
                                    book.InStock = book.InStock - orderItem.InStock;
                                    //update listbox
                                    Lst_Books.Items.RemoveAt(j);
                                    Lst_Books.Items.Insert(j, book);

                                    // fill a list for which books to update. then send it to backend to update books.json
                                    lstBooksToUpd.Add(new Book
                                    {
                                        Title = book.Title,
                                        Author = book.Author,
                                        Price = book.Price,
                                        InStock = book.InStock
                                    });

                                    custLetter.Append("- Required book: " + book.Title).AppendLine();
                                    custLetter.Append("  Auther: " + book.Author).AppendLine();
                                    custLetter.Append("  have been purchased").AppendLine().AppendLine();

                                }
                            }
                        }

                    }
                    #endregion

                    // to notify the customer of the result - TODO: to write a procedure to send it to customer by email for example 
                    MessageBox.Show(custLetter.ToString());

                    #region to update in stock balance in book.json as source data
                    if (lstBooksToUpd.Count > 0)
                    {
                        IBookstore bookstore = new BookstoreRefService();
                        bookstore.UpdateBooks(lstBooksToUpd.ToList());
                    }
                    #endregion

                    cancel(); // to clear inputs
                }
                catch (Exception ex)
                {
                    { MessageBox.Show(ex.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                }
            }
        }

        private void Btn_delOrder_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Book)
            {
                Book deleteItem = (Book)cmd.DataContext;
                if (MessageBox.Show("Are you sure to delete  ?", "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Lst_Order.Items.Remove(deleteItem);
                        Txt_TotalPrice.Text = (Convert.ToDecimal(Txt_TotalPrice.Text) - deleteItem.Price).ToString();
                    }
                    catch (Exception ex)
                    {
                        { MessageBox.Show(ex.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                    }
                }
            }
        }
        #endregion

        public void cancel()
        {
            Txt_Author.Text = "";
            Txt_InStock.Text = "0";
            Txt_Price.Text = "0,0";
            Txt_Title.Text = "";
            Grp_Input.Visibility = Visibility.Collapsed;

            Txt_OrderNo.Text = "";
            Txt_CustName.Text = "";
            Txt_CustEmail.Text = "";
            Txt_Qty.Text = "";
            Txt_TotalPrice.Text = "";
            Lst_Order.Items.Clear();
            Grp_Order.Visibility = Visibility.Collapsed;

            Btn_createOrder.IsEnabled = true;
            Btn_newBook.IsEnabled = true;
        }

    }
}
