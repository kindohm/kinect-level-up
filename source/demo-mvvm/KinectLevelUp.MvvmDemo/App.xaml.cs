using System.Windows;
using KinectLevelUp.MvvmDemo.ViewModels;

namespace KinectLevelUp.MvvmDemo
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
