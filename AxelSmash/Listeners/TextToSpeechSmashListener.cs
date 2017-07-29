using System;
using System.Threading.Tasks;
using Windows.Media.Audio;
using Windows.Media.SpeechSynthesis;
using AxelSmash.AudioGraphSpeech;
using AxelSmash.Smashes;

namespace AxelSmash.Listeners
{
    class TextToSpeechSmashListener : IObserver<IBabySmash>, IDisposable
    {
        private readonly AudioGraph graph;
        private readonly AudioDeviceOutputNode outputNode;

        public TextToSpeechSmashListener(AudioGraph graph, AudioDeviceOutputNode outputNode)
        {
            this.graph = graph;
            this.outputNode = outputNode;
        }

        private async Task SayText(string text)
        {
            var input = await graph.CreateSpeechInputNodeAsync(new SpeechSynthesizer(), text);
            input.AddOutgoingConnection(outputNode);
            input.SpeechCompleted += (sender, args) => input.Dispose();
            input.Start();
        }

        public async void OnNext(IBabySmash value)
        {
            // ReSharper disable once PossibleInvalidOperationException
            await SayText(value.Letter.Value.ToString());
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void Dispose()
        {
            
        }
    }
}