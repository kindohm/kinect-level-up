using System;
using System.Linq;
using Microsoft.Research.Kinect.Nui;
using System.Diagnostics;
using System.Windows;

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
                this.runtime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);
                this.runtime.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(
                    runtime_SkeletonFrameReady);
                this.runtime.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseDepth);
                runtime.DepthStream.Open(
                    ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException opEx)
            {
                MessageBox.Show("Kinect no worky.");
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
