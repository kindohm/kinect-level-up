using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.MvvmDemo.Services
{
    public class KinectService : IKinectService
    {
        Runtime runtime;

        public KinectService()
        {
            try
            {
                runtime = new Runtime();
                runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(runtime_SkeletonFrameReady);
                runtime.Initialize(RuntimeOptions.UseSkeletalTracking);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Kinect problem: " + ex.ToString());
                // kinect device not plugged in, or 
                // kinect driver issue
            }
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeleton = e.SkeletonFrame.Skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

            if (skeleton != null && this.SkeletonUpdated != null)
            {
                this.SkeletonUpdated(this,
                    new SkeletonUpdatedEventArgs() { HandRightJoint = skeleton.Joints[JointID.HandRight] });
            }

        }

        public void Shutdown()
        {
            if (runtime != null)
            {
                runtime.Uninitialize();
            }
        }

        public event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
