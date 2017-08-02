using System;
using System.Threading.Tasks;
using Windows.Media.Audio;
using Windows.Media.SpeechSynthesis;
using AxelSmash.Giggles;
using AxelSmash.Uwp.AudioGraphSpeech;

namespace AxelSmash.Uwp.Listeners
{
    class SpeechGigglePlayer : IObserver<SpeechGiggle>, IDisposable
    {
        private readonly AudioGraph graph;
        private readonly AudioDeviceOutputNode outputNode;

        public SpeechGigglePlayer(AudioGraph graph, AudioDeviceOutputNode outputNode)
        {
            this.graph = graph;
            this.outputNode = outputNode;
        }

        private async Task SayText(string text)
        {
            // https://blogs.windows.com/buildingapps/2016/05/23/using-speech-in-your-uwp-apps-from-talking-to-conversing/
            var input = await graph.CreateSpeechInputNodeAsync(new SpeechSynthesizer(), text);
            input.AddOutgoingConnection(outputNode);
            input.SpeechCompleted += (sender, args) => input.Dispose();
            input.Start();
        }

        public async void OnNext(SpeechGiggle value)
        {
            await SayText(value.Text);
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();

        public void Dispose()
        {
            
        }
    }
}