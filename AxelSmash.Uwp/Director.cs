using System;
using System.Reactive.Subjects;
using AxelSmash.Smashes;
using AxelSmash.Uwp.Giggles;
using Spectrum;

namespace AxelSmash.Uwp
{
    /// <summary>
    /// Director listen to smashes and produce giggles
    /// </summary>
    class Director : IObservable<IGiggle>, IObserver<IBabySmash>, IDisposable
    {
        private readonly Subject<IGiggle> giggles = new Subject<IGiggle>();

        public IDisposable Subscribe(IObserver<IGiggle> observer) => giggles.Subscribe(observer);

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void OnNext(IBabySmash value)
        {
            giggles.OnNext(new ShapeGiggle(Shape.Star, new Color.RGB(255, 0, 0)));

            if (value.Letter != null)
            {
                giggles.OnNext(new SpeechGiggle(value.Letter.ToString()));
            }
            else
            {
                giggles.OnNext(new RandomSoundGiggle());
            }
        }

        public void Dispose()
        {
            giggles?.Dispose();
        }
    }
}