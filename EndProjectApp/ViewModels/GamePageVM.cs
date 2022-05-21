using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EndProjectApp.DTO;
using EndProjectApp.Services;
using EndProjectApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.Linq;

namespace EndProjectApp.ViewModels
{
    class GamePageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region post
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
            if (p.LikeInPost.IsLiked)
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
            int index = PostList.IndexOf(p);
            PostList.RemoveAt(index);
            PostList.Insert(index, p);

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
            int index = PostList.IndexOf(p);
            PostList.RemoveAt(index);
            PostList.Insert(index, p);
        }
        public ICommand GoToPostPageCommand => new Command<PostDTO>(GoToPostPage);
        private void GoToPostPage(PostDTO p)
        {
            Page pa = new NavigationPage(new Views.PostPage());
            PostPageVM postPageVM = new PostPageVM();
            postPageVM.Post = p;
            pa.BindingContext = postPageVM;
            App.Current.MainPage.Navigation.PushAsync(pa);

        }
        public async void CreatePostList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<PostDTO> p = await proxy.GetAllPostsAsync();
            if (p != null)
            {
                foreach (PostDTO post in p)
                {
                    if(post.Post.TopicId == Topic.Id)
                    PostList.Add(post);
                }
            }
            else
            {

            }
        }
        #endregion
        private Topic topic;
        public Topic Topic
        {
            get { return topic; }
            set
            {
                if (Topic != value)
                {
                    topic = value;
                    OnPropertyChanged("Topic");
                }
            }
        }
        public GamePageVM()
        {
            PostList = new ObservableCollection<PostDTO>();
            ReviewList = new ObservableCollection<Review>();
            CreatePostList();
            CreateReviewList();




            IsRefresh = false;
            IsReviewRefresh = false;
        }
        private ObservableCollection<Review> reviewList;
        public ObservableCollection<Review> ReviewList
        {
            get { return reviewList; }
            set
            {
                if (ReviewList != value)
                {
                   reviewList = value;
                    OnPropertyChanged("ReviewList");

                }
            }
        }
        private bool isReviewRefresh;
        public bool IsReviewRefresh
        {
            get { return isReviewRefresh; }
            set
            {
                if (IsReviewRefresh != value)
                {
                    isReviewRefresh = value;
                    OnPropertyChanged("IsRefresh");
                }
            }
        }
        public ICommand RefreshReviewCommand => new Command(RefreshReview);

        private void RefreshReview()
        {
            ReviewList.Clear();
            CreateReviewList();

            IsReviewRefresh = false;
        }
        public ICommand GoToCreateReviewPageCommand => new Command(GoToCreateReviewPage);

        private void GoToCreateReviewPage()
        {
            Page pa = new NavigationPage(new Views.CreateReviewPage());
            CreateReviewPageVM createReviewPageVM = new CreateReviewPageVM { ThisTopic = Topic};
        
            pa.BindingContext = createReviewPageVM;
            App.Current.MainPage.Navigation.PushAsync(pa);

        }
        public async void CreateReviewList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Review> r = await proxy.GetReviewsAsync();
            if (r != null)
            {
                foreach (Review review in r)
                {
                    if (review.TopicId == Topic.Id)
                        ReviewList.Add(review);
                }
            }
        }
    }
}
