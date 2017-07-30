/*
 * Copyright (c) 2016 Ian Bebbington
 * MIT License
 * Blog: http://ian.bebbs.co.uk/posts/CombiningUwpSpeechSynthesizerWithAudioGraph
 * Src: https://github.com/ibebbs/BlogProjects/blob/master/UwpSpeechAudio/GraphExtensions.cs
 */

using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Audio;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.Media.SpeechSynthesis;

namespace AxelSmash.Uwp.AudioGraphSpeech
{
    public class AudioSpeechInputNode : IAudioInputNode
    {
        // File header byte count
        private const int StreamHeaderByteCount = 44;

        // Speech synthesis seems to create a 11.025khz, 32bit pcm output, not sure if this is constant or not
        private static readonly AudioEncodingProperties SpeechEncodingProperties = AudioEncodingProperties.CreatePcm(11025, 1, sizeof(float) * 8);

        private Stream stream;
        private AudioFrameInputNode frameInputNode;

        public event TypedEventHandler<AudioSpeechInputNode, Object> SpeechCompleted;

        public AudioSpeechInputNode(SpeechSynthesisStream stream, AudioGraph graph)
        {
            this.stream = stream.AsStreamForRead();
            this.stream.Seek(StreamHeaderByteCount, SeekOrigin.Begin);

            frameInputNode = graph.CreateFrameInputNode(SpeechEncodingProperties);
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

            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        private unsafe void QuantumStarted(AudioFrameInputNode sender, FrameInputNodeQuantumStartedEventArgs args)
        {
            var numSamplesNeeded = (uint)args.RequiredSamples;

            if (numSamplesNeeded != 0 && stream.Position < stream.Length)
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

                        for (var i = 0; i < bufferSize; i++)
                        {
                            if (stream.Position < stream.Length)
                            {
                                dataInBytes[i] = (byte)stream.ReadByte();
                            }
                            else
                            {
                                dataInBytes[i] = 0;
                            }
                        }
                    }
                }

                frameInputNode.AddFrame(frame);
            }
            else
            {
                SpeechCompleted?.Invoke(this, null);
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
            stream.Seek(StreamHeaderByteCount, SeekOrigin.Begin);

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