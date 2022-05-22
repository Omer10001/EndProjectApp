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
    class RepliesPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private CommentDTO comment;
        public CommentDTO Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }
        private ObservableCollection<CommentDTO> replyList;
        public ObservableCollection<CommentDTO> ReplyList
        {
            get { return replyList; }
            set
            {
                replyList = value;
                OnPropertyChanged("ReplyList");
            }
        }
        private async void CreateReplyList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<CommentDTO> c = await proxy.GetCommentsAsync();
            if (c != null)
            {
                foreach (CommentDTO comment in c)
                {
                    if (comment.Comment.RepliedToId == Comment.Comment.Id )
                        ReplyList.Add(comment);
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
            ReplyList.Clear();
            CreateReplyList();

            IsRefresh = false;
        }
        public RepliesPageVM()
        {
            ReplyList = new ObservableCollection<CommentDTO>();
            CreateReplyList();
            isRefresh = false;
        }
        private string userReply;
        public string UserReply
        {
            get { return userReply; }
            set
            {
                if (UserReply != value)
                {
                    userReply = value;
                    OnPropertyChanged("UserReply");
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
            if (c.Comment.RepliedToId!=null)
            {
              
                int index = ReplyList.IndexOf(c);
                ReplyList.RemoveAt(index);
                ReplyList.Insert(index, c);
            }
            

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
            if (c.Comment.RepliedToId != null)
            {

                int index = ReplyList.IndexOf(c);
                ReplyList.RemoveAt(index);
                ReplyList.Insert(index, c);
            }
        }
        public ICommand AddReplyCommand => new Command(AddReply);
        private async void AddReply()
        {
            try
            {
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                Comment c = new Comment { PostId = Comment.Comment.Post.Id, Text = UserReply, UserId = ((App)App.Current).CurrentUser.Id, TimeCreated = DateTime.Now, RepliedToId = Comment.Comment.Id };
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
