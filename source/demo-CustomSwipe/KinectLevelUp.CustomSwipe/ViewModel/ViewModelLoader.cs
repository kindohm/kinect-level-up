
namespace KinectLevelUp.CustomSwipe.ViewModel
{
    public class ViewModelLoader
    {
        static MainViewModel main;

        public ViewModelLoader()
        {
            main = new MainViewModel();
        }

        public MainViewModel Main
        {
            get { return main; }
        }

        public static void Cleanup()
        {
            main.Cleanup();
        }
    }
}
