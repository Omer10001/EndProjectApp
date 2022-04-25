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
        private ObservableCollection<User> allUsers;
        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set
            {
                if(allUsers!=value)
                {
                   allUsers = value;

                    OnPropertyChanged("AllUsers");
                }
                
            }
        }
        public AdminUsersPageVM()
        {
            if (!((App)App.Current).CurrentUser.IsAdmin)
                App.Current.MainPage.Navigation.PopAsync();
            GetUsers();
        }
        public async void GetUsers()
        {
           AllUsers = new ObservableCollection<User>();
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<User> u = await proxy.GetUsersAsync();
            if (u != null)
            {
                foreach (User user in u)
                {
                   AllUsers.Add(user);
                }
            }
        }
    }
}
