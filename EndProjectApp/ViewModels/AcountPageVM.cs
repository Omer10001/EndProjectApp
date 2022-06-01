
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
            if (string.IsNullOrEmpty(NewPassword))
            {
                ShowNewPasswordError = true;
                NewPasswordError = "please put Password";
            }
            else if (NewPassword.Length < 6)
            {
                ShowNewPasswordError = true;
                NewPasswordError = "Password must be at least 6 characters long";
            }
            else
                ShowNewPasswordError = false;
        }
        public AcountPageVM()
        {
            UserName = ((App)App.Current).CurrentUser.Name;
            Email = ((App)App.Current).CurrentUser.Email;
            DateCreated = ((App)App.Current).CurrentUser.DateCreated;
            Birthdate = ((App)App.Current).CurrentUser.BirthDate;
            LogoutCommand = new Command(Logout);
            ChangePasswordCommand = new Command(ChangePassword);
            IsAdmin = ((App)App.Current).CurrentUser.IsAdmin;
           
            AddGameCommand = new Command(AddGame);
        }
        public ICommand LogoutCommand { protected set; get; }
        private async void Logout()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            bool isFine = await proxy.LogoutAsync();
            if(isFine)
            {
                ((App)App.Current).CurrentUser = null;
                await App.Current.MainPage.DisplayAlert("success","Logout successful", "Okay");
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
        }
        public ICommand ChangePasswordCommand { protected set; get; }
        public async void ChangePassword()
        {
            ValidateNewPassword();
            if (!showNewPasswordError)
            {
                if(password != ((App)App.Current).CurrentUser.Password)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Password doesn't match", "Okay");
                    return;
                }
                else
                {
                    EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                    bool isFine = await proxy.ChangePasswordAsync(newPassword);
                    if (isFine)
                    {
                        await App.Current.MainPage.DisplayAlert("success", "Password Changed succesfully", "Okay");
                        NewPassword = "";
                        Password = "";
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("error", "A problem has occourred, try again later", "Okay");
                    }
                        
                }
               
                
            }
            
        }


        //admin stuff
        public bool IsAdmin { get; set; }

       
        public ICommand AddGameCommand { protected set; get; }
        private string gameName;
        public string GameName
        {
            get { return gameName; }
            set
            {
                gameName = value;

                OnPropertyChanged("GameName");
            }
        }
        private string gameDescription;
        public string GameDescription
        {
            get { return gameDescription; }
            set
            {
                gameDescription = value;

                OnPropertyChanged("GameDescription");
            }
        }
        public async void AddGame()
        {
            try
            {
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();

                Topic t = new Topic { Name = GameName , AboutText = gameDescription };
                Topic newTopic  = await proxy.AddGameAsync(t);
                if (newTopic!= null)
                {

                    if (this.imageFileResult != null)
                    {
                        bool success = await proxy.UploadImage(new FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"Topic{newTopic.Id}.jpg");
                    }
                    await App.Current.MainPage.DisplayAlert("Success", "Game added successfully", "Okay");
                    if (SetImageSourceEvent != null)
                        SetImageSourceEvent(null);
                    GameName = null;
                    GameDescription = null;
                    //refresh the main page 
                    int count = App.Current.MainPage.Navigation.NavigationStack.Count();
                    App.Current.MainPage.Navigation.InsertPageBefore(new Views.MainTabbedPage(), (Page)App.Current.MainPage.Navigation.NavigationStack[count-1]);

                }
                    
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Game already exists", "Okay");


            }
            catch( Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
        }
        private string topicImgSrc;
        public string TopicImgSrc
        {
            get => topicImgSrc;
            set
            {
                if (topicImgSrc != value)
                {
                    topicImgSrc = value;
                    OnPropertyChanged("TopicImgSrc");
                }
            }
        }
        ///The following command handle the pick photo button
        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick Image"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }

        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "Take Photo"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        public ICommand GoToAdminUsersPageCommand => new Command(GoToAdminUsersPage);
        private void GoToAdminUsersPage()
        {
            Page pa = new NavigationPage(new Views.AdminUsersPage());
            AdminUsersPageVM adminUsersPageVM = new AdminUsersPageVM ();
            pa.BindingContext = adminUsersPageVM;
            App.Current.MainPage.Navigation.PushAsync(pa);
            

        }
    }

}
