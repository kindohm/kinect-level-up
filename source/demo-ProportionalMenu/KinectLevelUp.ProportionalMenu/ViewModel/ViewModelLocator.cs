
using KinectLevelUp.ProportionalMenu.Services;
using GalaSoft.MvvmLight;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class ViewModelLocator
    {
        static MainViewModel main;
        static IKinectService kinectService;

        public ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                kinectService = new MockKinectService();
            }
            else
            {
                kinectService = new KinectService();
            }
            main = new MainViewModel(kinectService);
        }

        public MainViewModel Main
        {
            get
            {
                return main;
            }
        }

        public static void Cleanup()
        {
            kinectService.Cleanup();
            main.Cleanup();
        }
    }
}