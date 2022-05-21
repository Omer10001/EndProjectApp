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
    class CreateReviewPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private Topic topic;
        public Topic Topic
        {
            get { return topic; }
            set
            {
                topic = value;
                OnPropertyChanged("Topic");
            }
        }
        private Double score;
        public Double Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }
        private string reviewText;
        public string ReviewText
        {
            get { return reviewText; }
            set
            {
                reviewText = value;
                OnPropertyChanged("ReviewText");
            }
        }
        public ICommand CreateReviewCommand => new Command(CreateReview);
        public async void CreateReview()
        {
            try
            {
                Review review = new Review
                {
                    Score = (int)Score, //change this
                    Text = ReviewText,
                    TopicId = Topic.Id,
                    TimeCreated = DateTime.Now,
                    UserId = ((App)App.Current).CurrentUser.Id

                };
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                bool fine = await proxy.AddReviewAsync(review);
                if (fine)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Review created successfuly", "Okay");
                    await App.Current.MainPage.Navigation.PopAsync();


                }

                else
                    await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
           
        }
        public CreateReviewPageVM()
        {

        }
    }
}
