using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using KinectLevelUp.Speech.Infrastructure;
using KinectLevelUp.Speech.Views;
using System.Diagnostics;

namespace KinectLevelUp.Speech.Controls
{
    [TemplatePart(Name = ContentBName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = ContentAName, Type = typeof(ContentPresenter))]
    public class TransitionFrame : Frame
    {
        const string ContentBName = "ContentPresentationSiteB";
        const string ContentAName = "ContentPresentationSiteA";
        const string NormalState = "Normal";
        const string ForwardState = "Transition";
        const string BackwardState = "BackTransition";

        bool back; ContentPresenter contentPresentationSiteA;
        ContentPresenter contentPresentationSiteB;

        public TransitionFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionFrame), new FrameworkPropertyMetadata(typeof(TransitionFrame)));
            this.Navigating += new NavigatingCancelEventHandler(TransitionFrame_Navigating);
        }

        void TransitionFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            //var page = e.Content as TransitionPage;
            var content = this.Content as TransitionPage;

            if (content != null)
            {
                Debug.WriteLine(content.NavigationDirection);
                this.back = content.NavigationDirection == NavigationDirection.Previous;
            }

            //if (page != null)
            //{
            //    this.back = page.NavigationDirection == NavigationDirection.Previous;
            //}
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            contentPresentationSiteA = GetTemplateChild(ContentAName) as ContentPresenter;
            contentPresentationSiteB = GetTemplateChild(ContentBName) as ContentPresenter;

            if (contentPresentationSiteA != null)
            {
                contentPresentationSiteA.Content = Content;
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if ((contentPresentationSiteA != null) && (contentPresentationSiteB != null))
            {
                contentPresentationSiteA.Content = newContent;
                contentPresentationSiteB.Content = oldContent;

                VisualStateManager.GoToState(this, NormalState, false);
                if (this.back)
                {
                    VisualStateManager.GoToState(this, BackwardState, true);
                }
                else
                {
                    VisualStateManager.GoToState(this, ForwardState, true);
                }
            }

            // reset back flag
            this.back = false;
        }
    }
}
