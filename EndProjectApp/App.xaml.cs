using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EndProjectApp.Models;
using EndProjectApp.Views;


namespace EndProjectApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        //The current logged in user
        public User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CreatePostPage());
            CurrentUser = null;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
