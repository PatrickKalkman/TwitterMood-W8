using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TwitterMood.Management;
using TwitterMood.Management.Moods;
using TwitterMood.Moods;
using Win8nl.Services;
using Windows.ApplicationModel.Resources;

namespace TwitterMood.ViewModel
{
    public class MoodDetailViewModel : ViewModelBase
    {
        private readonly TwitterResponseStorage responseStorage;
        private readonly INavigationService navigationService;
        private readonly CurrentMoodMapper currentMoodMapper;
        private readonly ResourceLoader resourceLoader = new ResourceLoader();

        public MoodDetailViewModel()
        {
            this.TwitterMoodResult = new ObservableCollection<Status>();
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
                //this.TwitterMoodResult.Add(new Status() { from_user = "kalkie", text = "bla blab alb lorem ipsum en nog maar een taantal tot 1540", profile_image_url = "http://si0.twimg.com/profile_images/1211755241/jolanda_brils_normal.jpg" });
        }

        [PreferredConstructor]
        public MoodDetailViewModel(TwitterResponseStorage responseStorage, INavigationService navigationService, CurrentMoodMapper currentMoodMapper)
        {
            this.responseStorage = responseStorage;
            this.navigationService = navigationService;
            this.currentMoodMapper = currentMoodMapper;
            Messenger.Default.Register<MoodSelectedEvent>(this, OnMoodSelectedEvent);
            Title = resourceLoader.GetString("TwitterMoodDetail");
        }

        private void OnMoodSelectedEvent(MoodSelectedEvent moodSelectedEvent)
        {
            this.selectedMood = moodSelectedEvent.Mood;
            RootObject twitterResultForMood;
            Title = resourceLoader.GetString("TwitterMoodDetail") + " " + this.selectedMood.MoodString;
            MoodBase mood = this.currentMoodMapper.Map(this.selectedMood);

            if (responseStorage.TryGetValue(mood.Key, out twitterResultForMood))
            {
                this.TwitterMoodResult = new ObservableCollection<Status>(twitterResultForMood.statuses);
            }
        }

        private MoodDataItem selectedMood;

        private ObservableCollection<Status> twitterMoodResult;

        public ObservableCollection<Status> TwitterMoodResult
        {
            get { return twitterMoodResult; }
            set
            {
                twitterMoodResult = value;
                this.RaisePropertyChanged(() => TwitterMoodResult);
            }
        }

        public ICommand GoBackCommand
        {
            get
            {
                return new RelayCommand(() => navigationService.GoBack());
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }
    }
}