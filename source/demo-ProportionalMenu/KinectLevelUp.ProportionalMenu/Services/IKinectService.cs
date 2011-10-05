using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public interface IKinectService
    {
        ImageFrame LastDepthFrame { get; }

        void Cleanup();

        event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
        event EventHandler<SwipeEventArgs> SwipeDetected;
    }
}
