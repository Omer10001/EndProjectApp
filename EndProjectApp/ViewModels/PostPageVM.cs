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
        private ObservableCollection<Comment> commentList;
        public ObservableCollection<Comment> CommentList
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
            List<Comment> c = await proxy.GetCommentsAsync();
            if (c != null)
            {
                foreach (Comment comment in c)
                {
                    if(comment.PostId == Post.Post.Id && comment.RepliedToId == null)
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
            CommentList = new ObservableCollection<Comment>();
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
        public ICommand AddCommentCommand => new Command(AddComment);

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



    }
}
