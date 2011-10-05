using System;

namespace KinectLevelUp.Speech.Services
{
    public class SpeechRecognizedEventArgs : EventArgs
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public float Confidence { get; set; }
    }
}
