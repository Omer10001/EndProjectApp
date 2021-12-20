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
    class SignUpPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
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
        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
                OnPropertyChanged("Birthdate");
            }
        }
        public async void Submit()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            User u = new User { Email = email, BirthDate = birthdate, Name = userName, DateCreated = DateTime.Now, IsAdmin = false, Password = password };
            bool isFine = await proxy.SignUpAsync(u);
            if (isFine)
            {
                Page p = new NavigationPage(new Views.LoginPage());

                await App.Current.MainPage.Navigation.PushAsync(p);

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "SignUp Failed", "Okey");
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public SignUpPageVM()
        {
            SubmitCommand = new Command(Submit);
        }
       
    }
}
