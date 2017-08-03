using System;
using AxelSmash.Giggles;

namespace AxelSmash
{
    public class GigglePlayers : IDisposable
    {
        public IObserver<WelcomeSoundGiggle> WelcomeSound { get; }
        public IObserver<RandomSoundGiggle> RandomSound { get; }
        public IObserver<ShapeGiggle> Shape { get; }
        public IObserver<SpeechGiggle> Speech { get; }

        public GigglePlayers(IObserver<RandomSoundGiggle> randomSound, IObserver<WelcomeSoundGiggle> welcomeSound, IObserver<ShapeGiggle> shape, IObserver<SpeechGiggle> speech)
        {
            RandomSound = randomSound;
            WelcomeSound = welcomeSound;
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