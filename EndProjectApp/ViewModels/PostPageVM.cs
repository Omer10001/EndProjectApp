using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EndProjectApp.Services;
using EndProjectApp.Models;
using EndProjectApp.DTO;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.Linq;

namespace EndProjectApp.ViewModels
{
    class PostPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private PostDTO post;
        public PostDTO Post
        {
            get { return post; }
            set
            {
                post = value;
                OnPropertyChanged("Post");
            }
        }
        private ObservableCollection<CommentDTO> commentList;
        public ObservableCollection<CommentDTO> CommentList
        {
            get { return commentList; }
            set
            {
                commentList = value;
                OnPropertyChanged("CommentList");
            }
        }
        private async void CreateCommentList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<CommentDTO> c = await proxy.GetCommentsAsync();
            if (c != null)
            {
                foreach (CommentDTO comment in c)
                {
                    if(comment.Comment.PostId == Post.Post.Id && comment.Comment.RepliedToId == null)
                    CommentList.Add(comment);
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
        public ICommand RefreshCommand => new Command(Refresh);

        private void Refresh()
        {
            CommentList.Clear();
            CreateCommentList();

            IsRefresh = false;
        }
        public PostPageVM()
        {
            CommentList = new ObservableCollection<CommentDTO>();
            CreateCommentList();
            isRefresh = false;
        }
        private string userComment;
        public string UserComment
        {
            get { return userComment; }
            set
            {
                if (UserComment != value)
                {
                    userComment = value;
                    OnPropertyChanged("UserComment");
                }
            }
        }
        public ICommand LikeCommentCommand => new Command<CommentDTO>(LikeComment);
        private async void LikeComment(CommentDTO c)
        {
            if (c.LikeInComment.IsLiked)
            {

                c.LikeInComment.IsLiked = false;
                c.Comment.NumOfLikes--;


            }
            else
            {
                if (c.LikeInComment.IsDisliked)
                {
                    c.LikeInComment.IsDisliked = false;
                    c.Comment.NumOfLikes++;

                }
                c.LikeInComment.IsLiked = true;
                c.Comment.NumOfLikes++;

            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();

            await proxy.LikeComment(c);
            int index = CommentList.IndexOf(c);
            CommentList.RemoveAt(index);
            CommentList.Insert(index, c);

        }
        public ICommand DislikeCommentCommand => new Command<CommentDTO>(DislikeComment);
        private async void DislikeComment(CommentDTO c)
        {
            if (c.LikeInComment.IsDisliked)
            {
                c.LikeInComment.IsDisliked = false;
                c.Comment.NumOfLikes++;

            }
            else
            {
                if (c.LikeInComment.IsLiked)
                {
                    c.LikeInComment.IsLiked = false;
                    c.Comment.NumOfLikes--;

                }
                c.LikeInComment.IsDisliked = true;
                c.Comment.NumOfLikes--;

            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            await proxy.LikeComment(c);
            int index = CommentList.IndexOf(c);
            CommentList.RemoveAt(index);
            CommentList.Insert(index, c);
        }
        public ICommand AddCommentCommand => new Command(AddComment);
        public ICommand LikePostCommand => new Command(LikePost);
        private async void LikePost()
        {
            if (Post.LikeInPost.IsLiked)
            {

                Post.LikeInPost.IsLiked = false;
                Post.Post.NumOfLikes--;


            }
            else
            {
                if (Post.LikeInPost.IsDisliked)
                {
                    Post.LikeInPost.IsDisliked = false;
                    Post.Post.NumOfLikes++;

                }
                Post.LikeInPost.IsLiked = true;
                Post.Post.NumOfLikes++;

            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();

            await proxy.LikePost(Post);
           

        }
        public ICommand DislikePostCommand => new Command(DislikePost);
        private async void DislikePost()
        {
            if (Post.LikeInPost.IsDisliked)
            {
                Post.LikeInPost.IsDisliked = false;
                Post.Post.NumOfLikes++;

            }
            else
            {
                if (Post.LikeInPost.IsLiked)
                {
                    Post.LikeInPost.IsLiked = false;
                    Post.Post.NumOfLikes--;

                }
                Post.LikeInPost.IsDisliked = true;
                Post.Post.NumOfLikes--;

            }
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            await proxy.LikePost(Post);
            
        }
        private async void AddComment()
        {
            try
            {
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                Comment c = new Comment { PostId = Post.Post.Id, Text = UserComment, UserId = ((App)App.Current).CurrentUser.Id, TimeCreated = DateTime.Now };
                bool isFine = await proxy.AddCommentAsync(c);
                if (isFine)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Comment added successfully", "Okay");
                    Refresh();
                }

                else
                    await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");


            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
        }
        public ICommand GoToRepliesPageCommand => new Command<CommentDTO>(GoToRepliesPage);
        private void GoToRepliesPage(CommentDTO c)
        {
            Page pa = new NavigationPage(new Views.PostPage());
            RepliesPageVM repliesPageVM = new RepliesPageVM();
            repliesPageVM.Comment = c;
            pa.BindingContext = repliesPageVM;
            App.Current.MainPage.Navigation.PushAsync(pa);

        }


    }
}
