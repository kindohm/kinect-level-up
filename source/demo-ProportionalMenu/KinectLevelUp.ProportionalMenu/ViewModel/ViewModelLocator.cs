
using KinectLevelUp.ProportionalMenu.Services;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class ViewModelLocator
    {
        static MainViewModel main;
        static IKinectService kinectService;

        public ViewModelLocator()
        {
            kinectService = new KinectService();
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