using System;
using System.Threading.Tasks;
using Windows.Media.Audio;
using AxelSmash.Giggles;

namespace AxelSmash.Uwp.Listeners
{
    class SoundGigglePlayer : IObserver<SpeechGiggle>, IObserver<RandomSoundGiggle>, IObserver<WelcomeSoundGiggle>, IDisposable
    {
        private static readonly AudioGraphSettings GraphSettings
            = new AudioGraphSettings(Windows.Media.Render.AudioRenderCategory.Media);

        private readonly Task<(SpeechGigglePlayer, RandomSoundGigglePlayer)> init;
        private SpeechGigglePlayer textToSpeech;
        private RandomSoundGigglePlayer randomSound;

        public SoundGigglePlayer()
        {
            init = Init();
        }

        private AudioGraph graph;

        private async Task<(SpeechGigglePlayer, RandomSoundGigglePlayer)> Init()
        {
            graph = (await AudioGraph.CreateAsync(GraphSettings)).Graph;
            var outputNode = (await graph.CreateDeviceOutputNodeAsync()).DeviceOutputNode;
            graph.Start();

            textToSpeech = new SpeechGigglePlayer(graph, outputNode);
            randomSound = new RandomSoundGigglePlayer(graph, outputNode);

            return (textToSpeech, randomSound);
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();
        public async void OnNext(WelcomeSoundGiggle value) => (await init).Item2.OnNext(value);
        public async void OnNext(RandomSoundGiggle value) => (await init).Item2.OnNext(value);
        public async void OnNext(SpeechGiggle value) => (await init).Item1.OnNext(value);

        public void Dispose()
        {
            init?.Wait();

            randomSound?.Dispose();
            randomSound = null;

            textToSpeech?.Dispose();
            textToSpeech = null;

            graph?.Dispose();
            graph = null;
        }
    }
}