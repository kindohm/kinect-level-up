using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;
using Coding4Fun.Kinect.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using KinectLevelUp.ProportionalMenu.Services;

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
            this.kinectService.SkeletonUpdated += new EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
            this.kinectService.SwipeDetected += new EventHandler<SwipeEventArgs>(kinectService_SwipeDetected);
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();
            this.LoadMenuItems();

            this.imageWorker = new BackgroundWorker();
            this.imageWorker.DoWork += new DoWorkEventHandler(imageWorker_DoWork);
            this.imageWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(imageWorker_RunWorkerCompleted);
            this.imageWorker.RunWorkerAsync();

            Messenger.Default.Register<ItemMessage>(this, m =>
            {
                this.ItemMessageText = m.Text;
            });
        }


        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public const string ItemMessageTextPropertyName = "ItemMessageText";
        string itemMessageText = "Ready";
        public string ItemMessageText
        {
            get
            {
                return itemMessageText;
            }
            set
            {
                if (itemMessageText == value)
                {
                    return;
                }
                var oldValue = itemMessageText;
                itemMessageText = value;
                RaisePropertyChanged(ItemMessageTextPropertyName);
            }
        }

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
                e.HandRight.Position.Y < e.Head.Position.Y &&
                e.HandRight.Position.X > (e.HipRight.Position.X + .2f)
                )
            {
                // adjust Y values so that head is at zero of y axis
                var hand = e.HandRight.Position.Y - e.Head.Position.Y;
                var hip = e.HipRight.Position.Y - e.Head.Position.Y;
                var result = hand / hip;

                var fraction = 1d / this.MenuItems.Count;

                for (var i = 1; i <= this.MenuItems.Count; i++)
                {
                    if (fraction * i > result)
                    {
                        var item = this.MenuItems[i - 1];
                        if (selectedItem != item)
                        {
                            if (selectedItem != null)
                            {
                                this.selectedItem.IsSelected = false;
                            }
                            this.selectedItem = this.MenuItems[i - 1];
                            this.selectedItem.IsSelected = true;
                        }
                        break;
                    }
                }
            }
            //else
            //{
            //    if (this.selectedItem != null)
            //    {
            //        this.selectedItem.IsSelected = false;
            //        this.selectedItem = null;
            //    }
            //}
        }

        void imageWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.imageWorker.RunWorkerAsync();
        }

        void imageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(20);
            if (this.kinectService != null &&
                this.kinectService.LastDepthFrame != null &&
                App.Current != null)
            {
                App.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.DepthImage = this.kinectService.LastDepthFrame.ToBitmapSource();
                    }));
            }
        }

        void kinectService_SwipeDetected(object sender, SwipeEventArgs e)
        {
            Debug.WriteLine("Swipe detected.");
            if (this.selectedItem != null)
            {
                Debug.WriteLine("Executing command on item.");
                this.selectedItem.ItemCommand.Execute(null);
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