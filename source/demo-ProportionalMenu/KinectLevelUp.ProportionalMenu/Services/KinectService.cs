using System;
using System.Linq;
using System.Windows;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public class KinectService : IKinectService
    {
        bool initialized;
        Runtime runtime;
        HorizontalSwipe swipe;

        public KinectService()
        {
            try
            {
                this.runtime = new Runtime();
                this.runtime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);
                this.runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(
                    runtime_SkeletonFrameReady);
                this.runtime.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseDepthAndPlayerIndex);
                runtime.DepthStream.Open(
                    ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
                this.initialized = true;

                this.swipe = new HorizontalSwipe();
                this.swipe.SwipeCaptured += new EventHandler<SwipeEventArgs>(swipe_SwipeCaptured);
            }
            catch (InvalidOperationException opEx)
            {
                MessageBox.Show("Kinect no worky: " + opEx.ToString());
                // s'ok
            }
        }



        public ImageFrame LastDepthFrame { get; private set; }

        void runtime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.LastDepthFrame = e.ImageFrame;
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
                    this.swipe.AddSwipePoint(skeleton.Joints[JointID.HandRight].Position);

                    this.SkeletonUpdated(this,
                        new SkeletonUpdatedEventArgs()
                        {
                            HandRight = skeleton.Joints[JointID.HandRight],
                            HipRight = skeleton.Joints[JointID.HipRight],
                            ShoulderRight = skeleton.Joints[JointID.ShoulderRight],
                            Head = skeleton.Joints[JointID.Head]
                        });
                }
            }
        }

        void swipe_SwipeCaptured(object sender, SwipeEventArgs e)
        {
            if (this.SwipeDetected != null)
            {
                this.SwipeDetected(this, e);
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
        public event EventHandler<SwipeEventArgs> SwipeDetected;

    }
}
