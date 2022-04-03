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
    class MainPageVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ObservableCollection<PostDTO> postList;
        public ObservableCollection<PostDTO> PostList
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
            PostList = new ObservableCollection<PostDTO>();

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
        public ICommand LikeCommand => new Command<PostDTO>(Like);
        private async void Like(PostDTO p)
        {
            if(p.LikeInPost.IsLiked)
            {
               
                p.LikeInPost.IsLiked = false;
                p.Post.NumOfLikes--;
               
            }
            else
            {
                if (p.LikeInPost.IsDisliked)
                {
                    p.LikeInPost.IsDisliked = false;
                    p.Post.NumOfLikes++;
                   
                }
                p.LikeInPost.IsLiked = true;
                p.Post.NumOfLikes++;
               
            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            await proxy.LikePost(p);
        }
        public ICommand DislikeCommand => new Command<PostDTO>(Dislike);
        private async void Dislike(PostDTO p)
        {
            if (p.LikeInPost.IsDisliked)
            {
                p.LikeInPost.IsDisliked = false;
                p.Post.NumOfLikes++;
               
            }
            else
            {
                if (p.LikeInPost.IsLiked)
                {
                    p.LikeInPost.IsLiked = false;
                    p.Post.NumOfLikes--;
                   
                }
                p.LikeInPost.IsDisliked = true;
                p.Post.NumOfLikes--;
                
            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            await proxy.LikePost(p);
        }
        public ICommand GoToPostPageCommand => new Command<Post>(GoToPostPage);
        private void GoToPostPage(Post p)
        {
            Page pa = new NavigationPage(new Views.PostPage());
            PostPageVM postPageVM = new PostPageVM();
            postPageVM.Post = p;
            pa.BindingContext = postPageVM;
            App.Current.MainPage.Navigation.PushAsync(pa);
            
        }
        private async void CreatePostList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<PostDTO> p = await proxy.GetAllPostsAsync();
            if (p != null)
            {
                foreach (PostDTO post in p)
                {
                    PostList.Add(post);
                }
            }
            else
            {

            }
        }
        public ICommand GoToAccountCommand => new Command(GoToAccountPage);
        private void GoToAccountPage()
        {
            Page p = new NavigationPage(new Views.AcountPage());
            
            p.BindingContext = new AcountPageVM();
            App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
