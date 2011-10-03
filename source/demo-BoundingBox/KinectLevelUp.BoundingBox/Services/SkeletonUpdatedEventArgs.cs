using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.BoundingBox.Services
{
    public class SkeletonUpdatedEventArgs : EventArgs
    {
        public Joint HandRight { get; set; }
        public Joint Torso { get; set; }
    }
}
