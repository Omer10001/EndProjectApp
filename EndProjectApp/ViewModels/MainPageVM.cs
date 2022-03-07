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
using Xamarin.Essentials;
using System.Linq;
namespace EndProjectApp.ViewModels
{
    class MainPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ObservableCollection<Post> postList;
        public ObservableCollection<Post> PostList
        {
            get { return postList; }
            set
            {
                if (PostList != value)
                {
                    postList = value;
                    OnPropertyChanged("PostList");
                }
            }
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
        private bool isLiked;
        public bool IsLiked
        {
            get { return isLiked; }
            set
            {
                if (IsLiked != value)
                {
                    isLiked = value;
                    OnPropertyChanged("IsLiked");
                }
            }
        }
        private bool isDisliked;
        public bool IsDisliked
        {
            get { return isDisliked; }
            set
            {
                if (IsDisliked != value)
                {
                    isDisliked = value;
                    OnPropertyChanged("IsDisliked");
                }
            }
        }
        private Color likedColor;
        public Color LikedColor
        {
            get { return likedColor; }
            set
            {
                if (LikedColor != value)
                {
                    likedColor = value;
                    OnPropertyChanged("LikedColor");
                }
            }
        }
        private Color dislikedColor;
        public Color DislikedColor
        {
            get { return dislikedColor; }
            set
            {
                if (DislikedColor != value)
                {
                    dislikedColor = value;
                    OnPropertyChanged("DislikedColor");
                }
            }
        }
        public MainPageVM()
        {
            PostList = new ObservableCollection<Post>();

            CreatePostList();




            isRefresh = false;
        }
        public ICommand RefreshCommand => new Command(Refresh);

        private void Refresh()
        {
            PostList.Clear();           
            CreatePostList();

            IsRefresh = false;
        }
        public ICommand GoToPostPageCommand => new Command<Post>(GoToPostPage);
        private void GoToPostPage(Post p)
        {

        }
        private async void CreatePostList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Post> p = await proxy.GetAllPostsAsync();
            if (p != null)
            {
                foreach (Post post in p)
                {
                    PostList.Add(post);
                }
            }
            else
            {

            }
        }
    }
}
