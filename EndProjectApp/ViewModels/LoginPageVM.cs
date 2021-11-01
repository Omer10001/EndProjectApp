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
                ValidateEmail();
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
        private string emailError;
        public string EmailError
        {
            get { return emailError; }
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }
        private string passwordError;
        public string PasswordError
        {
            get { return passwordError; }
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }
        private bool showEmailError;
        public bool ShowEmailError
        {
            get { return showEmailError; }
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }
        private bool showPasswordError;
        public bool ShowPasswordError
        {
            get { return showPasswordError; }
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }
        public ICommand SubmitCommand { protected set; get; }

        public LoginPageVM()
        {
            SubmitCommand = new Command(OnSubmit);
            ShowPasswordError = false;
            ShowEmailError = false;
            EmailError = "please put email";
            PasswordError = "Please put password";
        }
        private void ValidateEmail()
        {
            ShowEmailError = string.IsNullOrEmpty(Email);
        }
        private void ValidatePassword()
        {
            ShowPasswordError = string.IsNullOrEmpty(Password);
        }
        private bool ValidateForm()
        {
            ValidateEmail();
            ValidatePassword();

            if (showEmailError || showPasswordError)
                return false;
            else
                return true;
                
        }
        public async void OnSubmit()
        {

            if(!ValidateForm())
            {
                await App.Current.MainPage.DisplayAlert("Error", "There is an issue with the informaion, please make sure...", "Okey");
            }
            else
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
}
