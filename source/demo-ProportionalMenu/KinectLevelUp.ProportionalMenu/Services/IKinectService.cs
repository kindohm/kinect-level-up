using System;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public interface IKinectService
    {
        void Cleanup();

        event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
