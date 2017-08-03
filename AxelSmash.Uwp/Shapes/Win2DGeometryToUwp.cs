using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Geometry;

namespace AxelSmash.Uwp.Shapes
{
    /// <summary>
    /// Convert a Win2D Geometry object to an UWP one
    /// </summary>
    class Win2DGeometryToUwp : ICanvasPathReceiver
    {
        public static PathGeometry Convert(CanvasGeometry canvasGeometry)
        {
            var converter = new Win2DGeometryToUwp();
            canvasGeometry.SendPathTo(converter);
            return converter.PathGeometry;
        }

        public PathGeometry PathGeometry { get; } = new PathGeometry();

        private PathFigure currentFigure;

        public void BeginFigure(System.Numerics.Vector2 startPoint, CanvasFigureFill figureFill)
        {
            currentFigure = new PathFigure
            {
                IsFilled = figureFill == CanvasFigureFill.Default,
                StartPoint = new Point(startPoint.X, startPoint.Y)
            };
        }

        public void AddArc(System.Numerics.Vector2 endPoint, float radiusX, float radiusY, float rotationAngle,
            CanvasSweepDirection sweepDirection, CanvasArcSize arcSize)
        {
            currentFigure.Segments.Add(new ArcSegment()
            {
                Point = new Point(endPoint.X, endPoint.Y),
                RotationAngle = rotationAngle,
                Size = new Size(radiusX, radiusY),
                IsLargeArc = arcSize == CanvasArcSize.Large,
                SweepDirection = sweepDirection == CanvasSweepDirection.Clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise
            });
        }

        private static Point ToPoint(System.Numerics.Vector2 v) => new Point(v.X, v.Y);

        public void AddCubicBezier(System.Numerics.Vector2 controlPoint1, System.Numerics.Vector2 controlPoint2,
            System.Numerics.Vector2 endPoint)
        {
            currentFigure.Segments.Add(new BezierSegment()
            {
                Point1 = ToPoint(controlPoint1),
                Point2 = ToPoint(controlPoint2),
                Point3 = ToPoint(endPoint)
            });
        }

        public void AddLine(System.Numerics.Vector2 endPoint)
        {
            currentFigure.Segments.Add(new LineSegment()
            {
                Point = ToPoint(endPoint)
            });
        }

        public void AddQuadraticBezier(System.Numerics.Vector2 controlPoint, System.Numerics.Vector2 endPoint)
        {
            currentFigure.Segments.Add(new QuadraticBezierSegment()
            {
                Point1 = ToPoint(controlPoint),
                Point2 = ToPoint(endPoint),
            });
        }

        public void SetFilledRegionDetermination(CanvasFilledRegionDetermination filledRegionDetermination)
        {
            switch (filledRegionDetermination)
            {
                case CanvasFilledRegionDetermination.Alternate:
                    PathGeometry.FillRule = FillRule.EvenOdd;
                    break;
                case CanvasFilledRegionDetermination.Winding:
                    PathGeometry.FillRule = FillRule.Nonzero;
                    break;
            }
            
        }

        public void SetSegmentOptions(CanvasFigureSegmentOptions figureSegmentOptions)
        {
        }

        public void EndFigure(CanvasFigureLoop figureLoop)
        {
            currentFigure.IsClosed = figureLoop == CanvasFigureLoop.Closed;
            PathGeometry.Figures.Add(currentFigure);
            currentFigure = null;
        }
    }
}