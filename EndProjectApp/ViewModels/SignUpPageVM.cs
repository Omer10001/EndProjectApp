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
        private bool showBirthdateError;
        public bool ShowBirthdateError
        {
            get { return showBirthdateError; }
            set
            {
                showBirthdateError = value;
                OnPropertyChanged("ShowBirthdateError");
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
        private string birthdateError;
        public string BirthdateError
        {
            get { return birthdateError; }
            set
            {
                birthdateError = value;
                OnPropertyChanged("BirthdateError");
            }
        }
        private void ValidateEmail()
        {
            ShowEmailError = string.IsNullOrEmpty(Email)  ;
            if (showEmailError)
                EmailError = "please put email";
        }
        private void ValidatePassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                ShowPasswordError = true;
                PasswordError = "please put Password";
            }
            else if (Password.Length < 6 )
            {
                ShowPasswordError = true;
                PasswordError = "Password must be at least 6 characters long";
            }
            else
                ShowPasswordError = false;
        }
        private void ValidateUserName()
        {
           
            if (string.IsNullOrEmpty(UserName))
            {
                ShowUserNameError = true;
                UserNameError = "please put Username";
            }
            else if (UserName.Length < 2 || UserName.Length > 19)
            {
                ShowUserNameError = true;
                UserNameError = "Username must be between 3 to 20 characters long";
            }
            else
                ShowUserNameError = false;
       
        }
        private void ValidateBirthdate()
        {
            if (Birthdate.AddYears(13) > DateTime.Now)
            {
                BirthdateError = "you must be older than 13";
                ShowBirthdateError = true;
            }
            else if (Birthdate.AddYears(100) < DateTime.Now)
            {
                BirthdateError = "please put a valid date";
                ShowBirthdateError = true;
            }
            else
                ShowBirthdateError = false;
            
        }
        private bool ValidateForm()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateUserName();
            ValidateBirthdate();

            if (showEmailError || showPasswordError || showUserNameError|| showBirthdateError)
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
                ValidateUserName();
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
                ValidatePassword();
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
                ValidateBirthdate();
                OnPropertyChanged("Birthdate");
            }
        }
        private async void Submit()
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
                    await App.Current.MainPage.DisplayAlert("Error", "SignUp Failed", "Okay");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "SignUp Failed, make sure all fields are good", "Okay");
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public SignUpPageVM()
        {
            SubmitCommand = new Command(Submit);
            birthdate = new DateTime(2000, 1, 1);
        }
       
    }
}
