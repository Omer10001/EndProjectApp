using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EndProjectApp.Services;
using EndProjectApp.Models;
using System.Collections.ObjectModel;
using EndProjectApp.DTO;
using Xamarin.Essentials;
using System.Linq;
namespace EndProjectApp.ViewModels
{
    class SearchPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private List<Post> allPosts;
        
        private ObservableCollection<PostDTO> filteredPosts;
        public ObservableCollection<PostDTO> FilteredPosts
        {
            get { return filteredPosts; }
            set
            {
                if (FilteredPosts != value)
                {
                    filteredPosts = value;
                    OnPropertyChanged("FilteredPosts");

                }
            }
        }
        private bool isRefresh;
        public bool IsRefresh
        {
            get { return isRefresh; }
            set
            {
                
                if (IsRefresh != value)
                {
                    isRefresh = value;
                    OnPropertyChanged("IsRefresh");
                }
            }
        }
    }
}
