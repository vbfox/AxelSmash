using System;
using System.Threading.Tasks;
using Windows.Media.Audio;
using AxelSmash.Giggles;

namespace AxelSmash.Uwp.Listeners
{
    class SoundGigglePlayer : IObserver<SpeechGiggle>, IObserver<RandomSoundGiggle>, IDisposable
    {
        private static readonly AudioGraphSettings GraphSettings
            = new AudioGraphSettings(Windows.Media.Render.AudioRenderCategory.Media);

        private readonly Task init;
        private SpeechGigglePlayer textToSpeech;
        private RandomSoundGigglePlayer randomSound;

        public SoundGigglePlayer()
        {
            init = Init();
        }

        private AudioGraph graph;

        private async Task Init()
        {
            graph = (await AudioGraph.CreateAsync(GraphSettings)).Graph;
            var outputNode = (await graph.CreateDeviceOutputNodeAsync()).DeviceOutputNode;
            graph.Start();

            textToSpeech = new SpeechGigglePlayer(graph, outputNode);
            randomSound = new RandomSoundGigglePlayer(graph, outputNode);
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();
        public void OnNext(RandomSoundGiggle value)
        {
            randomSound.OnNext(value);
        }

        public void OnNext(SpeechGiggle value)
        {
            textToSpeech.OnNext(value);
        }

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