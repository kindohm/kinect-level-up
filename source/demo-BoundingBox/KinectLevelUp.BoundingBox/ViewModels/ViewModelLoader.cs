using System.ComponentModel;
using System.Windows;
using KinectLevelUp.BoundingBox.Services;

namespace KinectLevelUp.BoundingBox.ViewModels
{
    public class ViewModelLoader
    {
        static MainViewModel main;
        static BoundingBoxViewModel boundingBox;
        static IKinectService kinectService;

        public ViewModelLoader()
        {
            var prop = DesignerProperties.IsInDesignModeProperty;

            var isInDesignMode
            = (bool)DependencyPropertyDescriptor
            .FromProperty(prop, typeof(FrameworkElement))
            .Metadata.DefaultValue;

            if (isInDesignMode)
            {
                kinectService = new MockKinectService();
            }
            else
            {
                kinectService = new KinectService();
            }

            main = new MainViewModel(kinectService);
            boundingBox = new BoundingBoxViewModel(kinectService);
        }

        public MainViewModel Main
        {
            get { return main; }
        }

        public BoundingBoxViewModel BoundingBox
        {
            get { return boundingBox; }
        }

        public static void Cleanup()
        {
            main.Cleanup();
            kinectService.Shutdown();
        }
    }
}
