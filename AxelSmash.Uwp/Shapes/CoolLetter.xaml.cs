using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AxelSmash.Uwp.Shapes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoolLetter : Page
    {
        public CoolLetter()
        {
            this.InitializeComponent();
        }

        public static Geometry MakeCharacterGeometry(char character)
        {
            /*
            var fontFamily = new FontFamily(Settings.Default.FontFamily);
            /*
            var typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Heavy, FontStretches.Normal);
            var formattedText = new FormattedText(
                character.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                300,
                Brushes.Black);
            var g = CanvasGeometry.CreateCircle(new CanvasControl(), new System.Numerics.Vector2(0, 0), 100); 
            g.SendPathTo(
            return formattedText.BuildGeometry(new Point(0, 0)).GetAsFrozen() as Geometry;
            */

            var cc = new CanvasControl();
            var g = CanvasGeometry.CreateText(
                new CanvasTextLayout(new CanvasDevice(false),
                "Hello world",
                new CanvasTextFormat() { FontFamily = "Consolas", FontSize = 200}, float.MaxValue, float.MaxValue));
            return CanvasGeometryToUwp.Convert(g);
        }

        class CanvasGeometryToUwp : ICanvasPathReceiver
        {
            public static PathGeometry Convert(CanvasGeometry canvasGeometry)
            {
                var converter = new CanvasGeometryToUwp();
                canvasGeometry.SendPathTo(converter);
                return converter.PathGeometry;
            }

            public PathGeometry PathGeometry { get; } = new PathGeometry();

            private PathFigure currentFigure;
            public void BeginFigure(System.Numerics.Vector2 startPoint, CanvasFigureFill figureFill)
            {
                currentFigure = new PathFigure();
                currentFigure.IsClosed = true;
                currentFigure.IsFilled = figureFill == CanvasFigureFill.Default;
                currentFigure.StartPoint = new Point(startPoint.X, startPoint.Y);
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
                
            }

            public void SetSegmentOptions(CanvasFigureSegmentOptions figureSegmentOptions)
            {
                
            }

            public void EndFigure(CanvasFigureLoop figureLoop)
            {
                PathGeometry.Figures.Add(currentFigure);
                currentFigure = null;
            }
        }
    }
}
