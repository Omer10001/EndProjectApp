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
    class MainTabbedPageVM
    {
        public List<Page> Pages;

        public MainTabbedPageVM()
        {
            Pages = new List<Page>();
           Pages.Add(  new Views.MainPage());
            Pages.Add(new Views.CreatePostPage());

        }
    }
}
