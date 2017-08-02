using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using AxelSmash.Colors;
using AxelSmash.Giggles;
using AxelSmash.Uwp.Shapes;
using JetBrains.Annotations;

namespace AxelSmash.Uwp.Listeners
{
    class ShapesGigglePlayer : IObserver<ShapeGiggle>, IDisposable
    {
        private readonly Canvas canvas;

        public ShapesGigglePlayer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        private readonly Random random = new Random();

        public void OnNext(ShapeGiggle value)
        {
            canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ShowSmash(value)).Ignore();
        }

        private static Color GetSaturatedColor(Hsl baseColor, double luminosity)
        {
            var rgb = new Hsl(baseColor.H, 1, luminosity).ToRgb();
            return Color.FromArgb(255, rgb.R, rgb.G, rgb.B);
        }

        private static LinearGradientBrush GetSaturatedLinearGradient(Hsl baseColor)
        {
            var brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(baseColor, 0.3), Offset = 0 });
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(baseColor, 0.5), Offset = 0.5 });
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(baseColor, 0.7), Offset = 1 });
            return brush;
        }

        private void ShowSmash(ShapeGiggle giggle)
        {
            var shapeControl = GetShapeControl(giggle);
            if (shapeControl == null)
            {
                return;
            }

            shapeControl.Name = Guid.NewGuid().ToString();
            shapeControl.Width = 150;
            shapeControl.Height = 150;

            canvas.Children.Add(shapeControl);
            shapeControl.Measure(new Size(canvas.ActualWidth, canvas.ActualHeight));
            var x = random.Next(0, (int)(canvas.ActualWidth - shapeControl.DesiredSize.Width));
            var y = random.Next(0, (int)(canvas.ActualHeight - shapeControl.DesiredSize.Height));
            
            Canvas.SetLeft(shapeControl, x);
            Canvas.SetTop(shapeControl, y);

            var storyboard = Animation.CreateDoubleAnimation(shapeControl,
                "Opacity",
                new Duration(TimeSpan.FromSeconds(5)), 1, 0);
            storyboard.Begin();
        }

        [CanBeNull]
        private static Control GetShapeControl(ShapeGiggle giggle)
        {
            var brush = GetSaturatedLinearGradient(giggle.Color);
            switch (giggle.Shape)
            {
                case Shape.Star:
                    return new CoolStar(brush);

                default:
                    return null;
            }
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void Dispose()
        {

        }
    }
}