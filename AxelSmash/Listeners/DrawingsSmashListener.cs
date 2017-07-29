using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using AxelSmash.Shapes;
using AxelSmash.Smashes;
using static Spectrum.Color;

namespace AxelSmash.Listeners
{
    class DrawingsSmashListener : IObserver<IBabySmash>, IDisposable
    {
        private readonly Canvas canvas;

        public DrawingsSmashListener(Canvas canvas)
        {
            this.canvas = canvas;
        }

        private readonly Random random = new Random();

        public void OnNext(IBabySmash value)
        {
            canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ShowSmash).Ignore();
        }

        private void ShowSmash()
        {
            var cool = new CoolStar();

            var hue = random.Next(0, 360);
            var start = new HSL(hue, 1, 0.4).ToRGB();
            var end = new HSL(hue, 1, 0.7).ToRGB();
            cool.GradientStart = Color.FromArgb(255, start.R, start.G, start.B);
            cool.GradientEnd = Color.FromArgb(255, end.R, end.G, end.B);

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