using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.BoundingBox.Services
{
    public class MockKinectService : IKinectService
    {
        public double BoundsWidth
        {
            get;
            set;
        }

        public double BoundsDepth
        {
            get;
            set;
        }

        public double MinDistanceFromCamera
        {
            get;
            set;
        }

        public bool UserIsInRange
        {
            get;
            set;
        }

        public bool IsInBoundingBoxConfigMode
        {
            get;
            set;
        }

        public ImageFrame LastDepthFrame { get; set; }
        public ImageFrame LastVideoFrame { get; set;  }

        public void Shutdown()
        {
        }

        public event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
