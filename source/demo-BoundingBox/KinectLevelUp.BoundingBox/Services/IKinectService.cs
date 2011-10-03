using System;

namespace KinectLevelUp.BoundingBox.Services
{
    public interface IKinectService
    {
        double BoundsWidth { get; set; }
        double BoundsDepth { get; set; }
        double MinDistanceFromCamera { get; set; }
        bool UserIsInRange { get; set; }
        bool IsInBoundingBoxConfigMode { get; set; }

        void Shutdown();
        event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
