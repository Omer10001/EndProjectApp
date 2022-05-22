using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using EndProjectApp.Services;
using EndProjectApp.Models;
using Xamarin.Essentials;
using System.Linq;

namespace EndProjectApp.ViewModels
{
    class CreatePostPageVM
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
       private Topic pickedTopic;
        public Topic PickedTopic
        {
            get { return pickedTopic; }
            set
            {
                if(pickedTopic != value)
                {
                    pickedTopic = value;

                    OnPropertyChanged("PickedTopic");
                }
               
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;

                    OnPropertyChanged("Title");
                }
            }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;

                    OnPropertyChanged("Text");
                }
            }
        }
        private ObservableCollection<Topic> topicList;
        public ObservableCollection<Topic> TopicList
        {
            get { return topicList; }
            set
            {
                if(topicList != value)
                {
                    topicList = value;

                    OnPropertyChanged("TopicList");
                }
               
            }
        }
        public ICommand CreatePostCommand { protected set; get; }
        public CreatePostPageVM()
        {
            TopicList = new ObservableCollection<Topic>();

            CreateTopicList();
            CreatePostCommand = new Command(CreatePost);



            
        }
        public async void CreatePost()
        {
            try
            {
                Post p = new Post
                {
                    TopicId = pickedTopic.Id,
                    NumOfLikes = 0,
                    Text = text,
                    TimeCreated = DateTime.Now,
                    Title = title,
                    UserId = ((App)App.Current).CurrentUser.Id,






                };
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                bool fine = await proxy.CreatePostAsync(p);
                if (fine)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Post created successfuly", "Okay");
                    Title = string.Empty;
                    Text = string.Empty;
                    PickedTopic = null;

                    
                }
                   
                else
                    await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
            }
           
        }
        private async void CreateTopicList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Topic> t = await proxy.GetTopics();
            if (t != null)
            {
                foreach (Topic topic in t)
                {
                    topicList.Add(topic);
                }
            }
            else
            {

            }
        }
        private string postImgSrc;

        public string PostImgSrc
        {
            get => postImgSrc;
            set
            {
                postImgSrc = value;
                OnPropertyChanged("PostImgSrc");
            }
        }
        ///The following command handle the pick photo button
        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick Image"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }

        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "Take Photo"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
    }
}
