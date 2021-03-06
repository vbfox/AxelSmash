﻿using System;
using Windows.Media.Audio;
using AxelSmash.Giggles;

namespace AxelSmash.Uwp.Listeners
{
    class RandomSoundGigglePlayer : IObserver<RandomSoundGiggle>, IObserver<WelcomeSoundGiggle>, IDisposable
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

        public RandomSoundGigglePlayer(AudioGraph graph, AudioDeviceOutputNode outputNode)
        {
            
        }

        public string GetRandomSoundFile()
        {
            return Sounds[random.Next(0, Sounds.Length)];
        }

        public async void OnNext(RandomSoundGiggle value)
        {
            await Audio.PlayWavResource(GetRandomSoundFile());
        }

        public void OnCompleted() => Dispose();

        public void OnError(Exception error) => Dispose();
        public async void OnNext(WelcomeSoundGiggle value)
        {
            await Audio.PlayWavResource("EditedJackPlaysBabySmash.wav");
        }

        public void Dispose()
        {
            
        }
    }
}
