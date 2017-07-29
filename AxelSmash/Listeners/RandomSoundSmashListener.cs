using System;
using AxelSmash.Smashes;
using Windows.Storage.Streams;
using Windows.Media;
using Windows.Foundation;
using Windows.Media.Audio;

namespace AxelSmash.Listeners
{
    class RandomSoundSmashListener : IObserver<IBabySmash>, IDisposable
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

        public RandomSoundSmashListener(AudioGraph graph, AudioDeviceOutputNode outputNode)
        {
            
        }

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
