using System;
using System.ComponentModel;
using System.Windows.Input;
using KinectLevelUp.CustomSwipe.Infrastructure;
using KinectLevelUp.CustomSwipe.Services;

namespace KinectLevelUp.CustomSwipe.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        const string PageUriFormat = "Page{0}.xaml";

        int index;
        IKinectService kinectService;

        public MainViewModel(IKinectService kinectService)
        {
            this.PreviousCommand = new RelayCommand(this.ExecutePreviousCommand);
            this.NextCommand = new RelayCommand(this.ExecuteNextCommand);

            this.kinectService = kinectService;
            this.kinectService.SwipeDetected += new EventHandler<SwipeEventArgs>(kinectService_SwipeDetected);

            this.index = 1;
            this.FrameSource = this.GetPageUri(this.index);
        }

        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        const string FrameSourceProperty = "FrameSource";
        string frameSource;
        public string FrameSource
        {
            get { return this.frameSource; }
            set
            {
                this.frameSource = value;
                this.OnPropertyChanged(FrameSourceProperty);
            }
        }

        const string NavigationDirectionProperty = "NavigationDirection";
        NavigationDirection navigationDirection;
        public NavigationDirection NavigationDirection
        {
            get { return this.navigationDirection; }
            set
            {
                this.navigationDirection = value;
                this.OnPropertyChanged(NavigationDirectionProperty);
            }
        }

        void kinectService_SwipeDetected(object sender, SwipeEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left)
            {
                this.GoToNextPage();
            }
            else if (e.Direction == SwipeDirection.Right)
            {
                this.GoToPreviousPage();
            }
        }

        void GoToNextPage()
        {
            if (this.index < 3)
            {
                this.NavigationDirection = NavigationDirection.Next;
                this.index++;
                var uri = this.GetPageUri(this.index);
                this.FrameSource = uri;
            }
        }

        void GoToPreviousPage()
        {
            if (this.index > 1)
            {
                this.NavigationDirection = NavigationDirection.Previous;
                this.index--;
                var uri = this.GetPageUri(this.index);
                this.FrameSource = uri;
            }
        }

        string GetPageUri(int index)
        {
            return string.Format(PageUriFormat, index.ToString());
        }

        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this,
                    new PropertyChangedEventArgs(property));
            }
        }

        void ExecutePreviousCommand()
        {
            this.GoToPreviousPage();
        }

        void ExecuteNextCommand()
        {
            this.GoToNextPage();
        }

        public void Cleanup()
        {
            this.kinectService.SwipeDetected -= this.kinectService_SwipeDetected;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
