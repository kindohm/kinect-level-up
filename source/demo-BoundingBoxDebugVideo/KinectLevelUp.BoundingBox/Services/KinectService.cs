using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.BoundingBox.Services
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
                runtime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);
                runtime.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_VideoFrameReady);
                runtime.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseColor);
                runtime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
                runtime.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Kinect problem: " + ex.ToString());
                // kinect device not plugged in, or 
                // kinect driver issue
            }
        }

        public double BoundsDepth { get; set; }
        public double BoundsWidth { get; set; }
        public double MinDistanceFromCamera { get; set; }
        public bool UserIsInRange { get; set; }
        public bool IsInBoundingBoxConfigMode { get; set; }
        public ImageFrame LastDepthFrame { get; private set; }
        public ImageFrame LastVideoFrame { get; private set; }

        void runtime_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.LastVideoFrame = e.ImageFrame;
        }

        void runtime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.LastDepthFrame = e.ImageFrame;
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeleton = e.SkeletonFrame.Skeletons
                                       .Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

            if (skeleton == null)
            {
                this.UserIsInRange = false;
                return;
            }

            this.UserIsInRange = this.GetUserIsInRange(skeleton.Joints[JointID.Spine].Position);

            if (this.SkeletonUpdated != null && (this.IsInBoundingBoxConfigMode || this.UserIsInRange))
            {
                this.SkeletonUpdated(this,
                    new SkeletonUpdatedEventArgs()
                    {
                        Torso = skeleton.Joints[JointID.Spine],
                        HandRight = skeleton.Joints[JointID.HandRight]
                    });
            }

        }

        public bool GetUserIsInRange(Vector torsoPosition)
        {
            return torsoPosition.Z > MinDistanceFromCamera &
                torsoPosition.Z < (MinDistanceFromCamera + BoundsDepth)
                & torsoPosition.X > -BoundsWidth / 2 &
                torsoPosition.X < BoundsWidth / 2;
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
