using System;
using System.Collections.Immutable;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AxelSmash.Giggles;
using AxelSmash.SmashSources;

namespace AxelSmash
{
    abstract class Controller : IDisposable
    {
        private readonly Director director = new Director();
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public void Start()
        {
            var source = new CompositeSmashSource(SmashSources);
            disposables.Add(source);
            disposables.Add(source.ObserveOn(Scheduler.Default).Subscribe(director));

            var players = GigglePlayers;
            disposables.Add(players);
            disposables.Add(director.OfType<RandomSoundGiggle>().Subscribe(players.RandomSound));
            disposables.Add(director.OfType<ShapeGiggle>().Subscribe(players.Shape));
            disposables.Add(director.OfType<SpeechGiggle>().Subscribe(players.Speech));
        }

        protected abstract ImmutableArray<ISmashSource> SmashSources { get; }
        protected abstract GigglePlayers GigglePlayers { get; }

        public void Dispose()
        {
            disposables.Dispose();
            director?.Dispose();
        }
    }
}