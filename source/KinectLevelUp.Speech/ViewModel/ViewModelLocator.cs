using GalaSoft.MvvmLight;
using KinectLevelUp.Speech.Services;

namespace KinectLevelUp.Speech.ViewModel
{
    public class ViewModelLocator
    {
        static IKinectService kinectService;
        static MainViewModel main;

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