using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.ProportionalMenu.Services
{
    public static class Extensions
    {
        const string Format = "0.000";

        public static bool IsZero(this Vector vector)
        {
            return vector.X == 0 & vector.Y == 0 & vector.Z == 0;
        }

        public static string ToCoordinateString(this Vector vector)
        {
            return string.Format("{0},{1},{2}", 
                vector.X.ToString(Format), vector.Y.ToString(Format), vector.Z.ToString(Format));
        }      

    }
}
