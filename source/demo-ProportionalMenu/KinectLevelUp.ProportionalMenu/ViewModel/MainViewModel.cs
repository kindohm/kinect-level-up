using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media.Imaging;
using Coding4Fun.Kinect.Wpf;
using GalaSoft.MvvmLight;
using KinectLevelUp.ProportionalMenu.Services;
using System.Diagnostics;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IKinectService kinectService;
        MenuItemViewModel selectedItem;
        BackgroundWorker imageWorker;

        public MainViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            this.kinectService.SkeletonUpdated += new System.EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();
            this.LoadMenuItems();

            this.imageWorker = new BackgroundWorker();
            this.imageWorker.DoWork += new DoWorkEventHandler(imageWorker_DoWork);
            this.imageWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(imageWorker_RunWorkerCompleted);
            this.imageWorker.RunWorkerAsync();
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public const string DepthImagePropertyName = "DepthImage";
        BitmapSource depthImage = null;
        public BitmapSource DepthImage
        {
            get
            {
                return depthImage;
            }
            set
            {
                if (depthImage == value)
                {
                    return;
                }
                var oldValue = depthImage;
                depthImage = value;
                RaisePropertyChanged(DepthImagePropertyName);
            }
        }

        public override void Cleanup()
        {
            this.kinectService.SkeletonUpdated -= this.kinectService_SkeletonUpdated;
        }

        void kinectService_SkeletonUpdated(object sender, SkeletonUpdatedEventArgs e)
        {
            this.AnalyzeSkeleton(e);
        }

        void AnalyzeSkeleton(SkeletonUpdatedEventArgs e)
        {
            if (e.HandRight.Position.Y > e.HipRight.Position.Y &&
                e.HandRight.Position.Y < e.ShoulderRight.Position.Y &&
                e.HandRight.Position.X > (e.HipRight.Position.X + .2f)
                )
            {
                // adjust Y values so that shoulder is at zero of y axis
                var hand = (int)Math.Abs(e.HandRight.Position.Y - e.ShoulderRight.Position.Y);
                var hip = (int)Math.Abs(e.HipRight.Position.Y - e.ShoulderRight.Position.Y);

                var remainder = (int)(hand % hip);

                Debug.WriteLine(
                    string.Format("Hand: {0}, Hip: {1}, Remainder: {2}",
                    hand, hip, remainder));

                this.selectedItem = this.MenuItems[remainder];
                this.selectedItem.IsSelected = true;
            }
            else
            {
                if (this.selectedItem != null)
                {
                    this.selectedItem.IsSelected = false;
                    this.selectedItem = null;
                }
            }
        }

        void imageWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.imageWorker.RunWorkerAsync();
        }

        void imageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(20);
            if (this.kinectService != null && this.kinectService.LastDepthFrame != null)
            {
                this.DepthImage = this.kinectService.LastDepthFrame.ToBitmapSource();
            }
        }

        void LoadMenuItems()
        {
            for (var i = 0; i < 4; i++)
            {
                var viewModel = new MenuItemViewModel();
                viewModel.Name = "Item " + i.ToString();
                this.MenuItems.Add(viewModel);
            }
        }
    }
}