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
using Xamarin.Essentials;
using System.Linq;
namespace EndProjectApp.ViewModels
{
    class MainPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private bool isRefresh;
        public ObservableCollection<Post> PostList { get; set; }
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

        public MainPageVM()
        {
            PostList = new ObservableCollection<Post>();

            CreatePostList();




            isRefresh = false;
        }
        private async void CreatePostList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Post> p = await proxy.GetAllPostsAsync();
            foreach (Post post in p)
            {
                PostList.Add(post);
            }
        }
    }
}
