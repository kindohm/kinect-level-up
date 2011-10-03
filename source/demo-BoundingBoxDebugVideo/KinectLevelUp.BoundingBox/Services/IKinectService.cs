using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.BoundingBox.Services
{
    public interface IKinectService
    {
        double BoundsWidth { get; set; }
        double BoundsDepth { get; set; }
        double MinDistanceFromCamera { get; set; }
        bool UserIsInRange { get; set; }
        bool IsInBoundingBoxConfigMode { get; set; }

        ImageFrame LastDepthFrame { get; }
        ImageFrame LastVideoFrame { get; }

        void Shutdown();
        event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
