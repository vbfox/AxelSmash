using System;
using AxelSmash.Smashes;

namespace AxelSmash.Listeners
{
    class AudioSmashListener : IObserver<IBabySmash>, IDisposable
    {
        private static readonly string[] Sounds = {
            "giggle.wav",
            "babylaugh.wav",
            "babygigl2.wav",
            "ccgiggle.wav",
            "laughingmice.wav",
            "scooby2.wav",
        };

        private readonly Random random = new Random();

        public string GetRandomSoundFile()
        {
            return Sounds[random.Next(0, Sounds.Length)];
        }

        public async void OnNext(IBabySmash value)
        {
            await Audio.PlayWavResource(GetRandomSoundFile());
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void Dispose()
        {
            
        }
    }
}
