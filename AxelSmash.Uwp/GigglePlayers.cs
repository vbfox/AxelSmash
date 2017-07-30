using System;
using AxelSmash.Giggles;

namespace AxelSmash
{
    class GigglePlayers : IDisposable
    {
        public IObserver<RandomSoundGiggle> RandomSound { get; }
        public IObserver<ShapeGiggle> Shape { get; }
        public IObserver<SpeechGiggle> Speech { get; }

        public GigglePlayers(IObserver<RandomSoundGiggle> randomSound, IObserver<ShapeGiggle> shape, IObserver<SpeechGiggle> speech)
        {
            RandomSound = randomSound;
            Shape = shape;
            Speech = speech;
        }

        public void Dispose()
        {
            RandomSound.OnCompleted();
            Shape.OnCompleted();
            Speech.OnCompleted();
        }
    }
}