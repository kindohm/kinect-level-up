using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.MvvmDemo.Services
{
    public class SkeletonUpdatedEventArgs : EventArgs
    {
        public Joint HandRightJoint { get; set; }
    }
}
