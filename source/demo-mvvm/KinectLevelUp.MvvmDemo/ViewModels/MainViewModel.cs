using System;
using System.ComponentModel;
using KinectLevelUp.MvvmDemo.Services;

namespace KinectLevelUp.MvvmDemo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        const double Scale = 200;

        IKinectService kinectService;

        public MainViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            kinectService.SkeletonUpdated += new EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
        }

        void kinectService_SkeletonUpdated(object sender, SkeletonUpdatedEventArgs e)
        {
            this.OffsetX = e.HandRightJoint.Position.X * Scale;
            this.OffsetY = e.HandRightJoint.Position.Y * Scale;
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
