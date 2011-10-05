using System;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public class MockKinectService : IKinectService
    {
        public Microsoft.Research.Kinect.Nui.ImageFrame LastDepthFrame
        {
            get;
            set; 
        }

        public void Cleanup()
        {

        }

        public event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
