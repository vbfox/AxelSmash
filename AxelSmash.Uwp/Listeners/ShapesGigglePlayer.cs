using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
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
            var control = GetControl(giggle);
            if (control == null)
            {
                return;
            }

            control.Width = 150;
            control.Height = 150;

            canvas.Children.Add(control);
            control.Measure(new Size(canvas.ActualWidth, canvas.ActualHeight));
            var x = random.Next(0, (int)(canvas.ActualWidth - control.DesiredSize.Width));
            var y = random.Next(0, (int)(canvas.ActualHeight - control.DesiredSize.Height));
            
            Canvas.SetLeft(control, x);
            Canvas.SetTop(control, y);
        }

        [CanBeNull]
        private static Control GetControl(ShapeGiggle giggle)
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