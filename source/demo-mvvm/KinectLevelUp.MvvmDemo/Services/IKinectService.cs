using System;

namespace KinectLevelUp.MvvmDemo.Services
{
    public interface IKinectService
    {
        void Shutdown();
        event EventHandler<SkeletonUpdatedEventArgs> SkeletonUpdated;
    }
}
