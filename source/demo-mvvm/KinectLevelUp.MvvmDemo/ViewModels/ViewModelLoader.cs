using KinectLevelUp.MvvmDemo.Services;

namespace KinectLevelUp.MvvmDemo.ViewModels
{
    public class ViewModelLoader
    {
        static MainViewModel main;
        static IKinectService kinectService;

        public ViewModelLoader()
        {
            kinectService = new KinectService(); // use IoC instead
            main = new MainViewModel(kinectService);
        }

        public MainViewModel Main
        {
            get { return main; }
        }

        public static void Cleanup()
        {
            main.Cleanup();
            kinectService.Shutdown();
        }
    }
}
