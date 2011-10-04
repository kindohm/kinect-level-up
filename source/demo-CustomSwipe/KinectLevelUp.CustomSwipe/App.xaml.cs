using System.Windows;
using KinectLevelUp.CustomSwipe.ViewModel;

namespace KinectLevelUp.CustomSwipe
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            ViewModelLoader.Cleanup();
            base.OnExit(e);
        }
    }
}
