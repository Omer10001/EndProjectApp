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
    class LoginPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public ICommand SubmitCommand { protected set; get; }

        public LoginPageVM()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public async void OnSubmit()
        {


            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            User user = await proxy.LoginAsync(Email, Password);
            if (user == null)
            {

                await App.Current.MainPage.DisplayAlert("Error", "Login Failed", "Okey");
            }
            else
            {

                App theApp = (App)App.Current;
                theApp.CurrentUser = user;

                Page p = new NavigationPage(new Views.AcountPage());
                App.Current.MainPage = p;



            }
        }
    }
}
