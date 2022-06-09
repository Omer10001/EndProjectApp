using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EndProjectApp.Services;
using EndProjectApp.Models;
using System.Collections.ObjectModel;
using EndProjectApp.DTO;
using Xamarin.Essentials;
using System.Linq;
namespace EndProjectApp.ViewModels
{
    class SearchPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        
        
        private ObservableCollection<Topic> filteredTopics;
        public ObservableCollection<Topic> FilteredTopics
        {
            get { return filteredTopics; }
            set
            {
                if (FilteredTopics != value)
                {
                    filteredTopics = value;
                    OnPropertyChanged("FilteredTopics");

                }
            }
        }
        public ObservableCollection<Topic> AllTopics
        {
            get; set;
        }
        private bool isRefresh;
        public bool IsRefresh
        {
            get { return isRefresh; }
            set
            {
                
                if (IsRefresh != value)
                {
                    isRefresh = value;
                    OnPropertyChanged("IsRefresh");
                }
            }
        }
        private string searchTerm;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                if (SearchTerm != value)
                {
                    searchTerm = value;
                    OnPropertyChanged("SearchTerm");
                    Search();

                }
            }
        }

        public SearchPageVM()
        {
            CreateTopicList();
            FilteredTopics = new ObservableCollection<Topic>();
        }
        public async void CreateTopicList()
        {
            AllTopics = new ObservableCollection<Topic>();
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Topic> t = await proxy.GetAllTopicsAsync();
            if (t != null)
            {
                foreach (Topic topic in t)
                {
                    AllTopics.Add(topic);
                }
            }
            
        }
        public void Search()
        {
            if (this.AllTopics == null || String.IsNullOrWhiteSpace(SearchTerm) || String.IsNullOrEmpty(SearchTerm))
                return;

            foreach (Topic t in this.AllTopics)
            {
               

                if (!this.FilteredTopics.Contains(t) &&
                    t.Name.Contains(SearchTerm))
                    this.FilteredTopics.Add(t);
                else if (this.FilteredTopics.Contains(t) &&
                    !t.Name.Contains(SearchTerm))
                    this.FilteredTopics.Remove(t);

                this.FilteredTopics = new ObservableCollection<Topic>(this.FilteredTopics.OrderBy(x => x.Name));
            }




        }
        public ICommand RefreshCommand => new Command(Refresh);

        private void Refresh()
        {
            FilteredTopics.Clear();
            CreateTopicList();
            Search();

            IsRefresh = false;
        }
        public ICommand GoToGamePageCommand => new Command<Topic>(GoToGamePage);
        private void GoToGamePage(Topic t)
        {
            Page pa = new NavigationPage(new Views.GamePage());
            GamePageVM gamePageVM = new GamePageVM { Topic = t };
            pa.BindingContext = gamePageVM;          
            App.Current.MainPage.Navigation.PushAsync(pa);
            SearchTerm = null;
            Refresh();

        }
    }
}
