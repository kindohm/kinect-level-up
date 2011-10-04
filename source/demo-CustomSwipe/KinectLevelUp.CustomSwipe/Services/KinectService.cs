using System;
using System.Linq;
using Microsoft.Research.Kinect.Nui;
using System.Diagnostics;

namespace KinectLevelUp.CustomSwipe.Services
{
    public class KinectService : IKinectService
    {
        bool initialized;
        Runtime runtime;
        HorizontalSwipe rightSwipe;
        HorizontalSwipe leftSwipe;

        public KinectService()
        {
            try
            {
                this.runtime = new Runtime();
                this.runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(runtime_SkeletonFrameReady);
                this.runtime.Initialize(RuntimeOptions.UseSkeletalTracking);
                this.initialized = true;
            }
            catch (InvalidOperationException ex)
            {
                // it's ok
                Debug.WriteLine(ex.ToString());
            }

            this.rightSwipe = new HorizontalSwipe();
            this.rightSwipe.SwipeCaptured += new EventHandler<SwipeEventArgs>(rightSwipe_SwipeCaptured);

            this.leftSwipe = new HorizontalSwipe();
            this.leftSwipe.SwipeCaptured += new EventHandler<SwipeEventArgs>(leftSwipe_SwipeCaptured);
        }

        void leftSwipe_SwipeCaptured(object sender, SwipeEventArgs e)
        {
            if (this.SwipeDetected != null)
            {
                e.Hand = Hand.Left;
                this.SwipeDetected(this, e);
            }
        }

        void rightSwipe_SwipeCaptured(object sender, SwipeEventArgs e)
        {
            if (this.SwipeDetected != null)
            {
                e.Hand = Hand.Right;
                this.SwipeDetected(this, e);
            }
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeleton = e.SkeletonFrame.Skeletons
                .Where(s => s.TrackingState == SkeletonTrackingState.Tracked)
                .FirstOrDefault();
            
            if (skeleton != null)
            {
                var rightHandPoint = skeleton.Joints[JointID.HandRight].Position;
                var leftHandPoint = skeleton.Joints[JointID.HandLeft].Position;
                this.leftSwipe.AddSwipePoint(leftHandPoint);
                this.rightSwipe.AddSwipePoint(rightHandPoint);
            }
        }

        public void Cleanup()
        {
            if (this.initialized)
            {
                runtime.Uninitialize();
            }
        }


        public event EventHandler<SwipeEventArgs> SwipeDetected;
    }
}
