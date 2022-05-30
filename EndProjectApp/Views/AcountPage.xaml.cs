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
    public partial class AcountPage : ContentPage
    {
        public AcountPage()
        {
            this.BindingContext = new AcountPageVM();
            ((AcountPageVM)this.BindingContext).SetImageSourceEvent += OnSetImageSource;
            InitializeComponent();
        }
        public void OnSetImageSource(ImageSource imgSource)
        {

            theImage.Source = imgSource;
        }
    }
}