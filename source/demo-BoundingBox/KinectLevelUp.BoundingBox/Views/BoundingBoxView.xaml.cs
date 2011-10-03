using System.ComponentModel;
using System.Windows;
using KinectLevelUp.BoundingBox.ViewModels;

namespace KinectLevelUp.BoundingBox.Views
{
    public partial class BoundingBoxView : Window
    {
        public BoundingBoxView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BoundingBox_Loaded);
        }

        void BoundingBox_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (BoundingBoxViewModel)this.DataContext;
            viewModel.OpenConfiguration();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var viewModel = (BoundingBoxViewModel)this.DataContext;
            viewModel.CloseConfiguration();
            base.OnClosing(e);
        }
    }

}
