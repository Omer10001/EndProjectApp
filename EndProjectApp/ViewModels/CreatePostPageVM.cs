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
    class CreatePostPageVM:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Validations
        private bool showPickedTopicError;
        public bool ShowPickedTopicError
        {
            get { return showPickedTopicError; }
            set
            {
                showPickedTopicError = value;
                OnPropertyChanged("ShowPickedTopicError");
            }
        }
        private bool showTitleError;
        public bool ShowTitleError
        {
            get { return showTitleError; }
            set
            {
                showTitleError = value;
                OnPropertyChanged("ShowTitleError");
            }
        }
        private bool showTextError;
        public bool ShowTextError
        {
            get { return showTextError; }
            set
            {
                showTextError = value;
                OnPropertyChanged("ShowTextError");
            }
        }
        private string pickedTopicError;
        public string PickedTopicError
        {
            get { return pickedTopicError; }
            set
            {
                pickedTopicError = value;
                OnPropertyChanged("PickedTopicError");
            }
        }
        private string titleError;
        public string TitleError
        {
            get { return titleError; }
            set
            {
                titleError = value;
                OnPropertyChanged("TitleError");
            }
        }
        private string textError;
        public string TextError
        {
            get { return textError; }
            set
            {
                textError = value;
                OnPropertyChanged("TextError");
            }
        }
        private void ValidateTopic()
        {
            if (PickedTopic == null)
                ShowPickedTopicError = true;
            else
                showPickedTopicError = false;
            if (showPickedTopicError)
                PickedTopicError = "Please Select Game";
        }
        private void ValidateTitle()
        {
            ShowTitleError = string.IsNullOrEmpty(title);
            if (showTitleError)
                TitleError = "please put a title";
        }
        private void ValidateText()
        {
            ShowTextError = string.IsNullOrEmpty(Text);
            if (showTextError)
                TextError = "please put Text";
        }
        
        private bool ValidateForm()
        {
            ValidateTopic();
            ValidateTitle();
            ValidateText();

            if (showPickedTopicError || showTitleError || showTextError )
                return false;
            else
                return true;

        }
        #endregion
        private Topic pickedTopic;
        public Topic PickedTopic
        {
            get { return pickedTopic; }
            set
            {
                if (pickedTopic != value)
                {
                    pickedTopic = value;
                    ValidateTopic();
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
                    ValidateTitle();

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
                    ValidateText();
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
                if (ValidateForm())
                {
                    Post p = new Post
                    {
                        TopicId = pickedTopic.Id,
                        NumOfLikes = 0,
                        Text = text,

                        Title = title,
                        UserId = ((App)App.Current).CurrentUser.Id,






                    };
                    EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                    Post newPost = await proxy.CreatePostAsync(p);
                    if (newPost != null)
                    {
                        if (this.imageFileResult != null)
                        {
                            bool success = await proxy.UploadImage(new FileInfo()
                            {
                                Name = this.imageFileResult.FullPath
                            }, $"Post{newPost.Id}.jpg");
                        }
                        await App.Current.MainPage.DisplayAlert("Success", "Post created successfuly", "Okay");
                        Title = string.Empty;
                        Text = string.Empty;

                        if (SetImageSourceEvent != null)
                            SetImageSourceEvent(null);
                        PickedTopic = null;

                        

                    }

                    else
                        await App.Current.MainPage.DisplayAlert("Error", "something went wrong", "Okay");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Create Post Failed, make sure all the fields are good", "okay");
                
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
                if (postImgSrc != value)
                {
                    postImgSrc = value;
                    OnPropertyChanged("PostImgSrc");
                }
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
