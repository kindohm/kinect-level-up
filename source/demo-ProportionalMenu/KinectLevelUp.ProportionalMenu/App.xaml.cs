using System.Windows;
using KinectLevelUp.ProportionalMenu.ViewModel;

namespace KinectLevelUp.ProportionalMenu
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
            base.OnExit(e);
        }
    }
}
