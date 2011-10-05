using System;

namespace KinectLevelUp.Speech.Services
{
    public class MockKinectService : IKinectService
    {
        public void AddGrammar(NuiGrammar grammar)
        {

        }

        public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

        public event EventHandler SpeechDetected;

        public event EventHandler SpeechRejected;

        public void Cleanup()
        {

        }
    }
}
