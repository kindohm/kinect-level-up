
using KinectLevelUp.CustomSwipe.Services;
namespace KinectLevelUp.CustomSwipe.ViewModel
{
    public class ViewModelLoader
    {
        static IKinectService kinectService;
        static MainViewModel main;

        public ViewModelLoader()
        {
            kinectService = new KinectService();

            main = new MainViewModel(kinectService);
        }

        public MainViewModel Main
        {
            get { return main; }
        }

        public static void Cleanup()
        {
            kinectService.Cleanup();
            main.Cleanup();
        }
    }
}
