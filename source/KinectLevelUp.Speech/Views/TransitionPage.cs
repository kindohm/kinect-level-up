using System.Windows;
using System.Windows.Controls;
using KinectLevelUp.Speech.Infrastructure;

namespace KinectLevelUp.Speech.Views
{
    public class TransitionPage : Page
    {
        public const string NavigationDirectionPropertyName = "NavigationDirection";
        public NavigationDirection NavigationDirection
        {
            get
            {
                return (NavigationDirection)GetValue(NavigationDirectionProperty);
            }
            set
            {
                SetValue(NavigationDirectionProperty, value);
            }
        }

        public static readonly DependencyProperty NavigationDirectionProperty = DependencyProperty.Register(
            NavigationDirectionPropertyName,
            typeof(NavigationDirection),
            typeof(TransitionPage),
            new UIPropertyMetadata(NavigationDirection.Next));
    }
}
