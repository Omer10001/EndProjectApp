
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
    class AcountPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public string UserName;

        public string Email;

        public DateTime DateCreated;
        public DateTime Birthdate;


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
        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                ValidateNewPassword();
                OnPropertyChanged("NewPassword");
            }
        }
        private string newPasswordError;
        public string NewPasswordError
        {
            get { return newPasswordError; }
            set
            {
                newPasswordError = value;
                OnPropertyChanged("NewPasswordError");
            }
        }
        private bool showNewPasswordError;
        public bool ShowNewPasswordError
        {
            get { return showNewPasswordError; }
            set
            {
                showNewPasswordError = value;
                OnPropertyChanged("ShowNewPasswordError");
            }
        }
        private void ValidateNewPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                ShowNewPasswordError = true;
                NewPasswordError = "please put Password";
            }
            else if (Password.Length < 6)
            {
                ShowNewPasswordError = true;
                NewPasswordError = "Password must be at least 6 characters long";
            }
            else
                ShowNewPasswordError = false;
        }


    }
}
