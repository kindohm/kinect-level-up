using System;
using System.ComponentModel;
using System.Windows.Input;
using KinectLevelUp.BoundingBox.Services;
using KinectLevelUp.BoundingBox.Views;

namespace KinectLevelUp.BoundingBox.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        const double Scale = 400;

        IKinectService kinectService;

        public MainViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            kinectService.SkeletonUpdated += new EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
            this.ShowBoundingBoxCommand = new RelayCommand(this.ExecuteShowBoundingBox);
        }

        void kinectService_SkeletonUpdated(object sender, SkeletonUpdatedEventArgs e)
        {
            if (this.kinectService.UserIsInRange)
            {
                this.OffsetX = e.HandRight.Position.X * Scale;
                this.OffsetY = e.HandRight.Position.Y * -Scale;
            }
        }

        const string OffsetXProperty = "OffsetX";
        double offsetX;
        public double OffsetX
        {
            get { return this.offsetX; }
            set
            {
                this.offsetX = value;
                this.OnPropertyChanged(OffsetXProperty);
            }
        }

        const string OffsetYProperty = "OffsetY";
        double offsetY;
        public double OffsetY
        {
            get { return this.offsetY; }
            set
            {
                this.offsetY = value;
                this.OnPropertyChanged(OffsetYProperty);
            }
        }

        public ICommand ShowBoundingBoxCommand { get; private set; }

        void ExecuteShowBoundingBox()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                new BoundingBoxView().Show();
            }));
        }

        void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Cleanup()
        {
            this.kinectService.SkeletonUpdated -= this.kinectService_SkeletonUpdated;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
