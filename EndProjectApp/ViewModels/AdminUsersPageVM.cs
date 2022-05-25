using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using EndProjectApp.Services;
using EndProjectApp.Models;
using Xamarin.Essentials;
using System.Linq;
namespace EndProjectApp.ViewModels
{
    class AdminUsersPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private ObservableCollection<User> userList;
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set
            {
                if(userList!=value)
                {
                   userList = value;

                    OnPropertyChanged("UserList");
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
        public ICommand RefreshCommand => new Command(Refresh);

        private void Refresh()
        {
            UserList.Clear();
            GetUsers();

            IsRefresh = false;
        }
        public AdminUsersPageVM()
        {
            if (!((App)App.Current).CurrentUser.IsAdmin)
                App.Current.MainPage.Navigation.PopAsync();
            GetUsers();
        }
        public async void GetUsers()
        {
           UserList = new ObservableCollection<User>();
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<User> u = await proxy.GetUsersAsync();
            if (u != null)
            {
                foreach (User user in u)
                {
                   UserList.Add(user);
                }
            }
        }
    }
}
