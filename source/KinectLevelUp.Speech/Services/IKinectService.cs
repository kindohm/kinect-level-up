using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectLevelUp.Speech.Services
{
    public interface IKinectService
    {
        void AddGrammar(NuiGrammar grammar);

        event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;
        event EventHandler SpeechDetected;
        event EventHandler SpeechRejected;

        void Cleanup();
    }
}
