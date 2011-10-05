using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.Recognition;
using Microsoft.Research.Kinect.Nui;
using Microsoft.Speech.AudioFormat;
using System.Threading;

namespace KinectLevelUp.Speech.Services
{
    public class KinectService : IKinectService
    {
        const string KinectSpeechRecognizer = "SR_MS_en-US_Kinect_10.0";

        KinectAudioSource kinectAudioSource;
        SpeechRecognitionEngine speechRecognitionEngine;
        NuiGrammar grammar;
        Thread speechThread;

        public void AddGrammar(NuiGrammar newGrammar)
        {
            if (this.grammar != null)
            {
                this.grammar = newGrammar;
                var g = ConvertToGrammar(grammar);
                speechRecognitionEngine.UnloadAllGrammars();
                speechRecognitionEngine.LoadGrammar(g);
            }
            else
            {

                this.grammar = newGrammar;

                if ((speechRecognitionEngine != null) && (speechRecognitionEngine.Grammars.Count == 1))
                    throw new InvalidOperationException("Only 1 grammar is supported in this release.");

                var recognizer = SpeechRecognitionEngine.InstalledRecognizers()
                    .Where(x => x.Id == KinectSpeechRecognizer).FirstOrDefault();

                if (recognizer == null)
                {
                    throw new InvalidOperationException("No speech recognizer found.");
                }

                speechRecognitionEngine = new SpeechRecognitionEngine(recognizer);

                var g = ConvertToGrammar(grammar);
                speechRecognitionEngine.LoadGrammar(g);

                speechRecognitionEngine.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(speechRecognitionEngine_SpeechDetected);
                speechRecognitionEngine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(speechRecognitionEngine_SpeechRecognitionRejected);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<Microsoft.Speech.Recognition.SpeechRecognizedEventArgs>(speechRecognitionEngine_SpeechRecognized);
                this.speechThread = new Thread(InitializeSpeech);
                this.speechThread.Start();
            }
        }

        void InitializeSpeech()
        {
            try
            {
                kinectAudioSource = new KinectAudioSource();
                kinectAudioSource.SystemMode = SystemMode.OptibeamArrayOnly;
                kinectAudioSource.FeatureMode = true;
                kinectAudioSource.AutomaticGainControl = false;
                kinectAudioSource.NoiseSuppression = true;
                kinectAudioSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
                var kinectStream = kinectAudioSource.Start();

                speechRecognitionEngine.SetInputToAudioStream(kinectStream, new SpeechAudioFormatInfo(
                                                               EncodingFormat.Pcm, 16000, 16, 1,
                                                               32000, 2, null));
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (InvalidOperationException)
            {
                // kinect unplugged or something. s'okay
            }
        }

        Grammar ConvertToGrammar(NuiGrammar grammar)
        {

            Grammar g = null;
            switch (grammar.GrammarType)
            {
                case NuiGrammar.NuiGrammarType.Basic:
                    var choices = new Choices();
                    foreach (var item in grammar.Items)
                    {
                        choices.Add(item.Text);
                    }
                    var gb = new GrammarBuilder();
                    gb.Culture = speechRecognitionEngine.RecognizerInfo.Culture;
                    gb.Append(choices);
                    g = new Grammar(gb);
                    break;

                case NuiGrammar.NuiGrammarType.Srgs:
                    throw new NotImplementedException();
                    break;
            }

            return g;
        }

        void speechRecognitionEngine_SpeechRecognized(object sender, Microsoft.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            var args = new SpeechRecognizedEventArgs { Text = e.Result.Text };
            if (e.Result.Semantics.Value != null)
                args.Value = e.Result.Semantics.Value.ToString();

            args.Confidence = e.Result.Confidence;

            this.OnSpeechRecognized(args);
        }

        void speechRecognitionEngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            this.OnSpeechRejected(EventArgs.Empty);
        }

        void speechRecognitionEngine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            this.OnSpeechDetected(EventArgs.Empty);
        }

        protected void OnSpeechRecognized(SpeechRecognizedEventArgs e)
        {
            if (this.SpeechRecognized != null)
                this.SpeechRecognized(this, e);
        }

        protected void OnSpeechDetected(EventArgs e)
        {
            if (this.SpeechDetected != null)
                this.SpeechDetected(this, e);
        }

        protected void OnSpeechRejected(EventArgs e)
        {
            if (this.SpeechRejected != null)
                this.SpeechRejected(this, e);
        }

        public void Cleanup()
        {
            if (speechRecognitionEngine != null)
            {
                speechRecognitionEngine.UnloadAllGrammars();
                speechRecognitionEngine.Dispose();
                speechRecognitionEngine = null;
            }

            if (kinectAudioSource != null)
            {
                kinectAudioSource.Stop();
                kinectAudioSource.Dispose();
                kinectAudioSource = null;
            }
        }

        public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;
        public event EventHandler SpeechDetected;
        public event EventHandler SpeechRejected;



    }
}
