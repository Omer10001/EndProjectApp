using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EndProjectApp.Services;
using EndProjectApp.Models;
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
        public ICommand GoToLoginCommand { protected set; get; }
        public ICommand GoToSignupCommand { protected set; get; }

        public void Login ()
        {
            Page p = new NavigationPage(new Views.LoginPage());
            p.Title = "login";
            App.Current.MainPage.Navigation.PushAsync(p);
        }
        public void SignUp()
        {
            Page p = new NavigationPage(new Views.SignUpPage());
            App.Current.MainPage.Navigation.PushAsync(p);
        }
        public MainPageVM()
        {
            GoToLoginCommand = new Command(Login);
            GoToSignupCommand = new Command(SignUp);
        }
    }
}
