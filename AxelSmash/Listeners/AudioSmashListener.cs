using System;
using System.Threading.Tasks;
using Windows.Media.Audio;
using AxelSmash.Smashes;

namespace AxelSmash.Listeners
{
    class AudioSmashListener : IObserver<IBabySmash>, IDisposable
    {
        private static readonly AudioGraphSettings GraphSettings
            = new AudioGraphSettings(Windows.Media.Render.AudioRenderCategory.Media);

        private readonly Task init;
        private TextToSpeechSmashListener textToSpeech;
        private RandomSoundSmashListener randomSound;

        public AudioSmashListener()
        {
            init = Init();
        }

        private AudioGraph graph;

        private async Task Init()
        {
            graph = (await AudioGraph.CreateAsync(GraphSettings)).Graph;
            var outputNode = (await graph.CreateDeviceOutputNodeAsync()).DeviceOutputNode;
            graph.Start();

            textToSpeech = new TextToSpeechSmashListener(graph, outputNode);
            randomSound = new RandomSoundSmashListener(graph, outputNode);
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void OnNext(IBabySmash value)
        {
            if (value.Letter.HasValue)
            {
                textToSpeech.OnNext(value);
            }
            else
            {
                randomSound.OnNext(value);
            }
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