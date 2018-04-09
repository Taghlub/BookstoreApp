using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Interface;
using BookstoreRef.Service;


namespace Bookstore.Viewer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
