using TwitterMood.Management;
using TwitterMood.ViewModel;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace TwitterMood
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
            this.CurrentMoodChart.Tapped += CurrentMoodChartOnTapped;
        }

        private void CurrentMoodChartOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            DataPointSeries series = (ColumnSeries)this.CurrentMoodChart.Series[0];
            if (series.SelectedItem != null)
            {
                var viewModel = this.DataContext as MainViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedMood = series.SelectedItem as MoodDataItem;
                }
            }
        }

        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainViewModel viewModel = this.DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.CurrentViewMode = ApplicationView.Value;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
