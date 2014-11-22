using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using TwitterMood.Management;
using TwitterMood.Management.Moods;
using TwitterMood.Moods;

using Win8nl.Services;
using Windows.Networking.Connectivity;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using System.Linq;

namespace TwitterMood.ViewModel
{
    /// <summary>
    /// The main view model of the main view.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IMoodManager moodManager;
        private readonly ILogger logger;
        private readonly INavigationService navigationService;
        private readonly MoodStorageManager moodStorageManager;
        private readonly CurrentMoodMapper currentMoodMapper;
        private readonly TileManager tileManager;
        private readonly Windows.ApplicationModel.Resources.ResourceLoader resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();

        private HistoricalMoodGraphData historicalMoodGraphData;

        private readonly DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(
            IMoodManager moodManager,
            MoodStorageManager moodStorageManager,
            CurrentMoodMapper currentMoodMapper,
            TileManager tileManager,
            ILogger logger,
            INavigationService navigationService)
        {
            this.moodManager = moodManager;
            this.HistoricalMoodGraphData = new ObservableCollection<MoodGraphDataItem>();
            this.CurrentMoodGraphData = new ObservableCollection<MoodDataItem>();
            this.moodStorageManager = moodStorageManager;
            this.currentMoodMapper = currentMoodMapper;
            this.tileManager = tileManager;
            this.logger = logger;
            this.navigationService = navigationService;

            CurrentMoodGraphData.Add(new MoodDataItem() {MoodString = "", CurrentValue = 0.0});

            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Start();

            moodManager.Initialize();

            Messenger.Default.Register<MoodsRetrieved>(this, OnMoodsRetrieved);
            Messenger.Default.Register<MoodRetrieved>(this, OnMoodRetrieved);
            Messenger.Default.Register<SettingsSavedEvent>(this, OnSettingsSavedEvent);
            SetCalculatingState();
            SetTitle(moodManager.GetAdditionalSearchTerm());
        }
        
        private void SetTitle(string addition)
        {
            if (string.IsNullOrEmpty(addition))
            {
                Title = resourceLoader.GetString("TwitterMoodGraph");
            }
            else
            {
                Title = resourceLoader.GetString("TwitterMoodGraph") + " (" + addition + ")";
            }
        }

        private void OnSettingsSavedEvent(SettingsSavedEvent settingsSavedEvent)
        {
            SetTitle(settingsSavedEvent.Settings.AdditionalSearchTerm);
        }

        private void SetCalculatingState()
        {
            InformationText = resourceLoader.GetString("Calculating");
        }

        private void OnMoodsRetrieved(MoodsRetrieved moods)
        {
            AddNewMoodMeasurementToCollection();
            LoadCurrentMoodGraphData();
            LoadHistoricalMoodGraphData();
            moodStorageManager.SaveMoodDataToStorage(historicalMoodGraphData);
            MoodBase currentMood = moodManager.GetDominantMood();
            InformationText = resourceLoader.GetString("CurrentMood") + currentMood.MoodName;
            tileManager.SendTileUpdate(currentMood);
        }

        private void OnMoodRetrieved(MoodRetrieved mood)
        {
            SetCalculatingState();
            this.ProgressValue = mood.NumberOfMoodsRetrieved;
        }


        public ObservableCollection<string> LogMessages
        {
            get { return logger.Messages; }
        }

        private void timer_Tick(object sender, object e)
        {
            SetTitle(moodManager.GetAdditionalSearchTerm());
            timer.Interval = TimeSpan.FromSeconds(20);
            logger.Clear();
            moodManager.Execute();
            RaisePropertyChanged(() => IsDisconnected);
        }

        private void AddNewMoodMeasurementToCollection()
        {
            historicalMoodGraphData.Add(moodManager.CreateGraphDataItem());
        }

        private MoodDataItem selectedMood;

        public MoodDataItem SelectedMood
        {
            set
            {
                selectedMood = value;
                this.navigationService.Navigate(typeof(MoodDetailView));
                Messenger.Default.Send(new MoodSelectedEvent() { Mood = selectedMood });
            }
        }

        private int progressValue;

        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                RaisePropertyChanged(() => ProgressValue);
            }
        }

        private string informationText;

        public string InformationText
        {
            get { return informationText; }
            set
            {
                informationText = value;
                RaisePropertyChanged(() => InformationText);
            }
        }

        public bool IsDisconnected
        {
            get
            {
                ConnectionProfile internetProfile = NetworkInformation.GetInternetConnectionProfile();
                if (internetProfile != null && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Called from load trigger from view.
        /// </summary>
        public void LoadInitialData()
        {
            historicalMoodGraphData = moodStorageManager.LoadMoodDataFromStorage().Result;
            LoadHistoricalMoodGraphData();
            LoadCurrentMoodGraphData();
        }

        private void LoadHistoricalMoodGraphData()
        {
            HistoricalMoodGraphData.Clear();
            if (historicalMoodGraphData != null)
            {
                foreach (MoodGraphDataItem moodGraphDataItem in historicalMoodGraphData)
                {
                    HistoricalMoodGraphData.Add(moodGraphDataItem);
                }
            }
        }

        private void LoadCurrentMoodGraphData()
        {
            if (historicalMoodGraphData != null)
            {
                MoodGraphDataItem mostRecentMood = historicalMoodGraphData.OrderByDescending(h => h.ItemDateTime).FirstOrDefault();
                if (mostRecentMood != null)
                {
                    List<MoodDataItem> currentMoods = currentMoodMapper.Map(mostRecentMood);
                    moodManager.SetCurrentMoods(currentMoods);

                    CurrentMoodGraphData.Clear();
                    foreach (MoodDataItem moodDataItem in currentMoods)
                    {
                        CurrentMoodGraphData.Add(moodDataItem);
                    }
                }
            }
        }

        private string currentState;
        public string CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (value != currentState)
                {
                    currentState = value;
                    RaisePropertyChanged(() => CurrentState);
                }
            }
        }

        public ObservableCollection<MoodGraphDataItem> HistoricalMoodGraphData { get; set; }

        public ObservableCollection<MoodDataItem> CurrentMoodGraphData { get; set; }

        private bool currentMoodTabSelected = true;

        public bool CurrentMoodTabSelected
        {
            get { return currentMoodTabSelected; }
            set
            {
                currentMoodTabSelected = value;
                RaisePropertyChanged(() => CurrentMoodTabSelected);
            }
        }

        private bool historicMoodTabSelected = false;

        public bool HistoricMoodTabSelected
        {
            get { return historicMoodTabSelected; }
            set
            {
                historicMoodTabSelected = value;
                RaisePropertyChanged(() => HistoricMoodTabSelected);
            }
        }

        private bool moodInPicturesTabSelected = false;

        public bool MoodInPicturesTabSelected
        {
            get { return moodInPicturesTabSelected; }
            set
            {
                moodInPicturesTabSelected = value;
                RaisePropertyChanged(() => MoodInPicturesTabSelected);
            }
        }

        private bool loggingTabSelected = false;

        public bool LoggingTabSelected
        {
            get { return loggingTabSelected; }
            set
            {
                loggingTabSelected = value;
                RaisePropertyChanged(() => LoggingTabSelected);
            }
        }

        private bool calculationLabelVisible = true;

        public bool CalculationLabelVisible 
        {
            get { return calculationLabelVisible; }        
            set
            {
                calculationLabelVisible = value;
                RaisePropertyChanged(() => CalculationLabelVisible);
            }
        }

        public ApplicationViewState CurrentViewMode
        {
            get { throw new NotImplementedException(); }
            set 
            {
                CalculationLabelVisible = value != ApplicationViewState.Snapped;
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