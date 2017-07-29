/*
 * Copyright (c) 2016 Ian Bebbington
 * MIT License
 * Blog: http://ian.bebbs.co.uk/posts/CombiningUwpSpeechSynthesizerWithAudioGraph
 * Src: https://github.com/ibebbs/BlogProjects/blob/master/UwpSpeechAudio/GraphExtensions.cs
 */

using System;
using System.Collections.Generic;
using Windows.Media;
using Windows.Media.Audio;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;

namespace AxelSmash.AudioGraphSpeech
{
    public class AudioWhiteNoiseInputNode : IAudioInputNode
    {
        private static readonly Random Random = new Random(DateTimeOffset.Now.Millisecond);
        private static readonly AudioEncodingProperties WhiteNoiseEncodingProperties = AudioEncodingProperties.CreatePcm(11025, 1, sizeof(float) * 8);

        private AudioFrameInputNode frameInputNode;

        public AudioWhiteNoiseInputNode(Windows.Media.Audio.AudioGraph graph)
        {
            frameInputNode = graph.CreateFrameInputNode(WhiteNoiseEncodingProperties);
            frameInputNode.QuantumStarted += QuantumStarted;
        }

        public void Dispose()
        {
            if (frameInputNode != null)
            {
                frameInputNode.QuantumStarted -= QuantumStarted;
                frameInputNode.Dispose();
                frameInputNode = null;
            }
        }

        private unsafe void QuantumStarted(AudioFrameInputNode sender, FrameInputNodeQuantumStartedEventArgs args)
        {
            var numSamplesNeeded = (uint)args.RequiredSamples;

            if (numSamplesNeeded != 0)
            {
                var bufferSize = numSamplesNeeded * sizeof(float);
                var frame = new AudioFrame(bufferSize);

                using (var buffer = frame.LockBuffer(AudioBufferAccessMode.Write))
                {
                    using (var reference = buffer.CreateReference())
                    {
                        // Get the buffer from the AudioFrame
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        ((IMemoryBufferByteAccess)reference).GetBuffer(out byte* dataInBytes, out uint _);

                        var dataInFloat = (float*)dataInBytes;

                        for (var i = 0; i < numSamplesNeeded; i++)
                        {
                            dataInFloat[i] = Convert.ToSingle(Random.NextDouble());
                        }
                    }
                }

                frameInputNode.AddFrame(frame);
            }
        }

        public void AddOutgoingConnection(IAudioNode destination)
        {
            frameInputNode.AddOutgoingConnection(destination);
        }

        public void AddOutgoingConnection(IAudioNode destination, double gain)
        {
            frameInputNode.AddOutgoingConnection(destination, gain);
        }

        public void RemoveOutgoingConnection(IAudioNode destination)
        {
            frameInputNode.RemoveOutgoingConnection(destination);
        }

        public void Start()
        {
            frameInputNode.Start();
        }

        public void Stop()
        {
            frameInputNode.Stop();
        }

        public void Reset()
        {
            frameInputNode.Reset();
        }

        public void DisableEffectsByDefinition(IAudioEffectDefinition definition)
        {
            frameInputNode.DisableEffectsByDefinition(definition);
        }

        public void EnableEffectsByDefinition(IAudioEffectDefinition definition)
        {
            frameInputNode.EnableEffectsByDefinition(definition);
        }

        public IReadOnlyList<AudioGraphConnection> OutgoingConnections => frameInputNode.OutgoingConnections;

        public bool ConsumeInput
        {
            get => frameInputNode.ConsumeInput;
            set => frameInputNode.ConsumeInput = value;
        }

        public IList<IAudioEffectDefinition> EffectDefinitions => frameInputNode.EffectDefinitions;

        public AudioEncodingProperties EncodingProperties => frameInputNode.EncodingProperties;

        public double OutgoingGain
        {
            get => frameInputNode.OutgoingGain;
            set => frameInputNode.OutgoingGain = value;
        }
    }
}