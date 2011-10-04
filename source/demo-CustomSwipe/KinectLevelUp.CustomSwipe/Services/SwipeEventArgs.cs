using System;

namespace KinectLevelUp.CustomSwipe.Services
{
    public class SwipeEventArgs : EventArgs
    {
        public SwipeDirection Direction { get; set; }
        public Hand Hand { get; set; }
    }
}
