using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndProjectApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EndProjectApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePostPage : ContentPage
    {
        public CreatePostPage()
        {
            this.BindingContext = new CreatePostPageVM(); 
            InitializeComponent();
        }
    }
}