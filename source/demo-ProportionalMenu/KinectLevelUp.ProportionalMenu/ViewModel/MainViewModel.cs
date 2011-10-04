using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using KinectLevelUp.ProportionalMenu.Services;
using System;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IKinectService kinectService;
        MenuItemViewModel selectedItem;

        public MainViewModel(IKinectService kinectService)
        {
            this.kinectService = kinectService;
            this.kinectService.SkeletonUpdated += new System.EventHandler<SkeletonUpdatedEventArgs>(kinectService_SkeletonUpdated);
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();
            this.LoadMenuItems();
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; }

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
                var hand = Math.Abs(e.HandRight.Position.Y - e.ShoulderRight.Position.Y);
                var hip = Math.Abs(e.HipRight.Position.Y - e.ShoulderRight.Position.Y);

                var remainder = (int)(hand % hip);
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