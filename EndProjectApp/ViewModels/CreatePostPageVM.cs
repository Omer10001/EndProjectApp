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
        public List<Topic> TopicList { get; set; }
        public CreatePostPageVM()
        {
            TopicList = new List<Topic>();

            CreateTopicList();




            
        }
        private async void CreateTopicList()
        {
            EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
            List<Topic> t = await proxy.GetTopics();
            if (t != null)
            {
                foreach (Topic topic in t)
                {
                    TopicList.Add(topic);
                }
            }
            else
            {

            }
        }
    }
}
