using GalaSoft.MvvmLight;
using KinectLevelUp.Speech.Services;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using KinectLevelUp.Speech.Infrastructure;

namespace KinectLevelUp.Speech.ViewModel
{   
    public class MainViewModel : ViewModelBase
    {
        const string NextCommandText = "Next";
        const string PreviousCommandText = "Previous";
        const string PageUriFormat = "Page{0}.xaml";

        int index;
        IKinectService kinectService;

        public MainViewModel(IKinectService kinectService)
        {
            this.Messages = new ObservableCollection<string>();

            this.kinectService = kinectService;
            this.kinectService.SpeechDetected += new EventHandler(kinectService_SpeechDetected);
            this.kinectService.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(kinectService_SpeechRecognized);
            this.kinectService.SpeechRejected += new EventHandler(kinectService_SpeechRejected);

            var grammar = new NuiGrammar();
            grammar.Items.Add(new NuiGrammarItem() { Text = NextCommandText });
            grammar.Items.Add(new NuiGrammarItem() { Text = PreviousCommandText });
            grammar.Items.Add(new NuiGrammarItem() { Text = "kinect" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "speech" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "mike hodnick" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "code camp" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "twin cities" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "twin cities code camp" });
            grammar.Items.Add(new NuiGrammarItem() { Text = "alpacas like to eat breath mints" });

            this.kinectService.AddGrammar(grammar);

            this.Messages.Add("Say something...");
            this.Messages.Add("Ready");

            this.index = 1;
            this.FrameSource = this.GetPageUri(this.index);

        }

        public ObservableCollection<string> Messages { get; private set; }

        public const string FrameSourcePropertyName = "FrameSource";
        string frameSource = string.Empty;
        public string FrameSource
        {
            get
            {
                return frameSource;
            }
            set
            {
                if (frameSource == value)
                {
                    return;
                }
                var oldValue = frameSource;
                frameSource = value;
                RaisePropertyChanged(FrameSourcePropertyName);
            }
        }

        public const string NavigationDirectionPropertyName = "NavigationDirection";
        NavigationDirection navigationDirection = NavigationDirection.Next;
        public NavigationDirection NavigationDirection
        {
            get
            {
                return navigationDirection;
            }
            set
            {
                if (navigationDirection == value)
                {
                    return;
                }
                var oldValue = navigationDirection;
                navigationDirection = value;
                RaisePropertyChanged(NavigationDirectionPropertyName);
            }
        }

        void GoToNextPage()
        {
            if (this.index < 3)
            {
                this.NavigationDirection = NavigationDirection.Next;
                this.index++;
                var uri = this.GetPageUri(this.index);
                this.FrameSource = uri;
            }
        }

        void GoToPreviousPage()
        {
            if (this.index > 1)
            {
                this.NavigationDirection = NavigationDirection.Previous;
                this.index--;
                var uri = this.GetPageUri(this.index);
                this.FrameSource = uri;
            }
        }

        string GetPageUri(int index)
        {
            return string.Format(PageUriFormat, index.ToString());
        }

        void kinectService_SpeechRejected(object sender, EventArgs e)
        {
            this.Messages.Insert(0, "Oh noes, speech rejected!");
        }

        void kinectService_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.Messages.Insert(0, "Speech recognized: \"" + e.Text + "\", Confidence: " + e.Confidence.ToString());

            if (e.Confidence > .96)
            {
                if (e.Text == NextCommandText)
                {
                    this.GoToNextPage();
                }
                else if (e.Text == PreviousCommandText)
                {
                    this.GoToPreviousPage();
                }
            }
        }

        void kinectService_SpeechDetected(object sender, EventArgs e)
        {
            // this happens too often to show on the screen
            Debug.WriteLine("Speech detected.");
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}