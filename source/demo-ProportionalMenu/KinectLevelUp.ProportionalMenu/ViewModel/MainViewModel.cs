using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using KinectLevelUp.ProportionalMenu.Services;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IKinectService kinectService;

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