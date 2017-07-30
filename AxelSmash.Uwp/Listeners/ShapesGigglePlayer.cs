using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using AxelSmash.Uwp.Giggles;
using AxelSmash.Uwp.Shapes;

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
            canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ShowSmash).Ignore();
        }

        private static Color GetSaturatedColor(double hue, double luminosity)
        {
            var rgb = new Spectrum.Color.HSL(hue, 1, luminosity).ToRGB();
            return Color.FromArgb(255, rgb.R, rgb.G, rgb.B);
        }

        private static LinearGradientBrush GetSaturatedLinearGradient(double hue)
        {
            var brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(hue, 0.3), Offset = 0 });
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(hue, 0.5), Offset = 0.5 });
            brush.GradientStops.Add(new GradientStop { Color = GetSaturatedColor(hue, 0.7), Offset = 1 });
            return brush;
        }

        private void ShowSmash()
        {
            var hue = random.Next(0, 360);
            var brush = GetSaturatedLinearGradient(hue);
            var cool = new CoolStar(brush);

            cool.Width = 150;
            cool.Height = 150;

            canvas.Children.Add(cool);
            cool.Measure(new Size(canvas.ActualWidth, canvas.ActualHeight));
            var x = random.Next(0, (int)(canvas.ActualWidth - cool.DesiredSize.Width));
            var y = random.Next(0, (int)(canvas.ActualHeight - cool.DesiredSize.Height));
            
            Canvas.SetLeft(cool, x);
            Canvas.SetTop(cool, y);
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void Dispose()
        {

        }
    }
}