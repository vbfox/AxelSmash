using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using AxelSmash.Shapes;
using AxelSmash.Smashes;

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
            var x = random.Next(0, (int)canvas.ActualWidth);
            var y = random.Next(0, (int)canvas.ActualHeight);
            var cool = new CoolStar();
            canvas.Children.Add(cool);
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