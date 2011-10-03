using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Kinect.Wpf;
using KinectLevelUp.BoundingBox.Services;

namespace KinectLevelUp.BoundingBox.ViewModels
{
    public class BoundingBoxViewModel : INotifyPropertyChanged
    {
        IKinectService kinectService;
        BackgroundWorker videoWorker;

        public BoundingBoxViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            this.kinectService.SkeletonUpdated += new EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
            this.BoundsDisplaySize = 300;
            this.BoundsDepth = .7d;
            this.BoundsWidth = .7d;
            this.MinDistanceFromCamera = 1.0d;

            this.videoWorker = new BackgroundWorker();
            this.videoWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(videoWorker_RunWorkerCompleted);
            this.videoWorker.DoWork += new DoWorkEventHandler(videoWorker_DoWork);
            this.videoWorker.RunWorkerAsync();
        }

        BitmapSource depthImage;
        public BitmapSource DepthImage
        {
            get
            {
                return this.depthImage;
            }
            set
            {
                this.depthImage = value;
                this.OnPropertyChanged("DepthImage");
            }
        }

        BitmapSource videoImage;
        public BitmapSource VideoImage
        {
            get
            {
                return this.videoImage;
            }
            set
            {
                this.videoImage = value;
                this.OnPropertyChanged("VideoImage");
            }
        }

        public double MinDistanceFromCamera
        {
            get { return this.kinectService.MinDistanceFromCamera; }
            set
            {
                this.kinectService.MinDistanceFromCamera = value;
                OnPropertyChanged("MinDistanceFromCamera");
            }
        }

        double _boundsDisplaySize;
        public double BoundsDisplaySize
        {
            get
            {
                return _boundsDisplaySize;
            }
            set
            {
                _boundsDisplaySize = value;
                OnPropertyChanged("BoundsDisplaySize");
            }
        }

        public double BoundsWidth
        {
            get { return this.kinectService.BoundsWidth; }
            set
            {
                this.kinectService.BoundsWidth = value;
                OnPropertyChanged("BoundsWidth");
            }
        }

        public double BoundsDepth
        {
            get { return this.kinectService.BoundsDepth; }
            set
            {
                this.kinectService.BoundsDepth = value;
                OnPropertyChanged("BoundsDepth");
            }
        }

        Color _userPointColor = Colors.Green;
        public Color UserPointColor
        {
            get { return _userPointColor; }
            set
            {
                if (_userPointColor != value)
                {
                    _userPointColor = value;
                    OnPropertyChanged("UserPointColor");
                }
            }
        }

        double _torsoOffsetX;
        public double TorsoOffsetX
        {
            get { return _torsoOffsetX; }
            set
            {
                _torsoOffsetX = value;
                OnPropertyChanged("TorsoOffsetX");
            }
        }

        double _torsoOffsetZ;
        public double TorsoOffsetZ
        {
            get { return _torsoOffsetZ; }
            set
            {
                _torsoOffsetZ = value;
                OnPropertyChanged("TorsoOffsetZ");
            }
        }

        bool _userIsInRange;
        public bool UserIsInRange
        {
            get { return _userIsInRange; }
            set
            {
                _userIsInRange = value;
                OnPropertyChanged("UserIsInRange");
            }
        }

        public void OpenConfiguration()
        {
            this.kinectService.IsInBoundingBoxConfigMode = true;
        }

        public void CloseConfiguration()
        {
            this.kinectService.IsInBoundingBoxConfigMode = false;
        }

        void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        void kinectService_SkeletonUpdated(object sender, SkeletonUpdatedEventArgs e)
        {
            UserPointColor = this.kinectService.UserIsInRange
                ? Color.FromArgb(255, 0, 255, 0) : Color.FromArgb(255, 255, 0, 0);

            TorsoOffsetX = (BoundsDisplaySize / 2) * e.Torso.Position.X / (BoundsWidth / 2);

            TorsoOffsetZ = (BoundsDisplaySize / 2) *
                (e.Torso.Position.Z - (MinDistanceFromCamera + BoundsDepth / 2))
                / (BoundsDepth / 2);
        }


        void videoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.videoWorker.RunWorkerAsync(); //infinitely do work
        }

        void videoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(20);
            if (this.kinectService != null)
            {
                if (this.kinectService.LastVideoFrame != null && this.kinectService.LastVideoFrame != null)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.DepthImage = this.kinectService.LastDepthFrame.ToBitmapSource();
                        this.VideoImage = this.kinectService.LastVideoFrame.ToBitmapSource();
                    }));
                }
            }
        }

        public void Cleanup()
        {
            this.kinectService.SkeletonUpdated -= kinectService_SkeletonUpdated;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
