using System;
using System.Reactive.Subjects;
using System.Threading;
using AxelSmash.Colors;
using AxelSmash.Giggles;
using AxelSmash.Smashes;

namespace AxelSmash
{
    /// <summary>
    /// Director listen to smashes and produce giggles
    /// </summary>
    public class Director : IObservable<IGiggle>, IObserver<IBabySmash>, IDisposable
    {
        private readonly Subject<IGiggle> giggles = new Subject<IGiggle>();

        public IDisposable Subscribe(IObserver<IGiggle> observer) => giggles.Subscribe(observer);

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(() => new Random());

        public void OnNext(IBabySmash value)
        {
            var hue = Random.Value.Next(0, 360);
            var color = new Hsl(hue, 1, 0.5);

            if (value.Letter != null)
            {
                giggles.OnNext(new ShapeGiggle(Shape.Letter, color, char.ToUpper(value.Letter.Value)));
                giggles.OnNext(new SpeechGiggle(value.Letter.ToString()));
            }
            else
            {
                giggles.OnNext(new ShapeGiggle(Shape.Star, color, null));
                giggles.OnNext(new RandomSoundGiggle());
            }
        }

        public void Dispose()
        {
            giggles?.Dispose();
        }
    }
}