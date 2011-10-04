using System;
using Microsoft.Research.Kinect.Nui;

namespace KinectLevelUp.CustomSwipe.Services
{
    public class HorizontalSwipe
    {
        Vector[] points;
        int last;
        int lineSize;
        float maxYDelta;
        float minXDelta;

        public int LineSize { get { return lineSize; } set { lineSize = value; Initialize(); } }
        public float MaxYDelta { get { return maxYDelta; } set { maxYDelta = value; Initialize(); } }
        public float MinXDelta { get { return minXDelta; } set { minXDelta = value; Initialize(); } }

        public HorizontalSwipe()
        {
            LineSize = 9;
            MaxYDelta = .10f;
            MinXDelta = .5f;
        }

        void Initialize()
        {
            last = LineSize - 1;
            points = new Vector[LineSize];
            for (int i = 0; i < LineSize; i++)
                points[i] = new Vector();
        }

        public bool AnalyzePoints(Vector[] points)
        {
            // assume anything right at the origin means
            // the point is not a real point
            if (points[0].IsZero() || points[last].IsZero())
                return false;

            var x1 = points[0].X;
            var y1 = points[0].Y;
            var x2 = points[last].X;
            var y2 = points[last].Y;

            if (Math.Abs(x1 - x2) < MinXDelta)
                return false;

            if (y1 - y2 > MaxYDelta)
                return false;

            for (var i = 1; i < LineSize - 2; i++)
            {
                if (points[i].X == 0)
                    return false;

                if (Math.Abs((points[i].Y - y1)) > MaxYDelta)
                    return false;

                var result =
                    (y1 - y2) * points[i].X +
                    (x2 - x1) * points[i].Y +
                    (x1 * y2 - x2 * y1);

                if (result > Math.Abs(result))
                {
                    return false;
                }
            }
            return true;
        }

        public void AddSwipePoint(Vector newPoint)
        {
            var max = LineSize - 1;
            for (var i = max; i > 0; i--)
            {
                points[i] = points[i - 1];
            }

            points[0] = newPoint;
            var isZero = points[0].IsZero();

            if (!isZero)
            {
                var result = AnalyzePoints(points);

                if (result)
                {

                    if (SwipeCaptured != null)
                    {
                        var direction = points[0].X > points[last].X ? SwipeDirection.Right : SwipeDirection.Left;
                        SwipeCaptured(null,
                            new SwipeEventArgs() { Direction = direction });
                    }

                    for (int i = 0; i < LineSize; i++)
                    {
                        points[i] = new Vector();
                    }
                }

            }
        }

        public event EventHandler<SwipeEventArgs> SwipeCaptured;
    }
}
