
using System;
namespace KinectLevelUp.CustomSwipe.Services
{
    public interface IKinectService
    {
        void Cleanup();
        event EventHandler<SwipeEventArgs> SwipeDetected;
    }
}
