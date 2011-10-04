using System;
using System.Linq;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public class KinectService : IKinectService
    {
        bool initialized;
        Runtime runtime;

        public KinectService()
        {
            try
            {
                this.runtime = new Runtime();
                this.runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(
                    runtime_SkeletonFrameReady);
                this.runtime.Initialize(RuntimeOptions.UseSkeletalTracking);
            }
            catch (InvalidOperationException opEx)
            {
                // s'ok
            }
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeleton = e.SkeletonFrame.Skeletons
                .Where(s => s.TrackingState == SkeletonTrackingState.Tracked)
                .FirstOrDefault();

            if (skeleton != null)
            {
                if (this.SkeletonUpdated != null)
                {
                    this.SkeletonUpdated(this,
                        new SkeletonUpdatedEventArgs()
                        {
                            HandRight = skeleton.Joints[JointID.HandRight],
                            HipRight = skeleton.Joints[JointID.HipRight],
                            ShoulderRight = skeleton.Joints[JointID.ShoulderRight]
                        });
                }
            }
        }

        public void Cleanup()
        {
            if (this.initialized)
            {
                this.runtime.Uninitialize();
            }
        }

        public event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
