using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public class SkeletonUpdatedEventArgs : EventArgs
    {
        public Joint HandRight { get; set; }
        public Joint ShoulderRight { get; set; }
        public Joint HipRight { get; set; }
        public Joint Head { get; set; }
    }
}
