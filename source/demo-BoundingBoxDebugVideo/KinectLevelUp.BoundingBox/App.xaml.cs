using System.Windows;
using KinectLevelUp.BoundingBox.ViewModels;

namespace KinectLevelUp.BoundingBox
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
