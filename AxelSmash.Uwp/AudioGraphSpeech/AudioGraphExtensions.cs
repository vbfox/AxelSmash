/*
 * Copyright (c) 2016 Ian Bebbington
 * MIT License
 * Blog: http://ian.bebbs.co.uk/posts/CombiningUwpSpeechSynthesizerWithAudioGraph
 * Src: https://github.com/ibebbs/BlogProjects/blob/master/UwpSpeechAudio/GraphExtensions.cs
 */

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.SpeechSynthesis;

namespace AxelSmash.AudioGraphSpeech
{
    public static class AudioGraphExtensions
    {
        private static async Task<AudioSpeechInputNode> CreateSpeechInputNode(Windows.Media.Audio.AudioGraph graph, SpeechSynthesizer synth, string text)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            return new AudioSpeechInputNode(stream, graph);
        }

        public static IAsyncOperation<AudioSpeechInputNode> CreateSpeechInputNodeAsync(this Windows.Media.Audio.AudioGraph graph, SpeechSynthesizer synth, string text)
        {
            return CreateSpeechInputNode(graph, synth, text).AsAsyncOperation();
        }

        private static Task<AudioWhiteNoiseInputNode> CreateWhiteNoiseInputNode(Windows.Media.Audio.AudioGraph graph)
        {
            return Task.FromResult(new AudioWhiteNoiseInputNode(graph));
        }

        public static IAsyncOperation<AudioWhiteNoiseInputNode> CreateWhiteNoiseInputNodeAsync(this Windows.Media.Audio.AudioGraph graph)
        {
            return CreateWhiteNoiseInputNode(graph).AsAsyncOperation();
        }
    }
}