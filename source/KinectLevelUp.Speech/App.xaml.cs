using System.Windows;
using KinectLevelUp.Speech.ViewModel;

namespace KinectLevelUp.Speech
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
