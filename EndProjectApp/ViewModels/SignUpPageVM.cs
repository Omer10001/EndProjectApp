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
        #region Validation
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
        private bool showUserNameError;
        public bool ShowUserNameError
        {
            get { return showUserNameError; }
            set
            {
                showUserNameError = value;
                OnPropertyChanged("ShowUserNameError");
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
        private string userNameError;
        public string UserNameError
        {
            get { return userNameError; }
            set
            {
                userNameError = value;
                OnPropertyChanged("UserNameError");
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
        private void ValidateEmail()
        {
            ShowEmailError = string.IsNullOrEmpty(Email);
            if (showEmailError)
                EmailError = "please put email";
        }
        private void ValidatePassword()
        {
            ShowPasswordError = string.IsNullOrEmpty(Password);
            if (showPasswordError)
                PasswordError = "Please put password";
        }
        private void ValidateUserName()
        {
            ShowUserNameError = string.IsNullOrEmpty(UserName);
            if (showUserNameError)
                userNameError = "please put userName";
        }
        private bool ValidateForm()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateUserName();

            if (showEmailError || showPasswordError || showUserNameError)
                return false;
            else
                return true;

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
            if (ValidateForm())
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
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "SignUp Failed, make sure all fields are good", "Okey");
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public SignUpPageVM()
        {
            SubmitCommand = new Command(Submit);
        }
       
    }
}
