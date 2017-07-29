using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using AxelSmash.Smashes;

namespace AxelSmash.SmashSources
{
    class CompositeSmashSource : ISmashSource
    {
        private ImmutableArray<ISmashSource> providers;

        public CompositeSmashSource(params ISmashSource[] sources)
        {
            this.providers = sources.ToImmutableArray();
        }

        public void Dispose()
        {
            foreach (var provider in providers)
                provider.Dispose();
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return providers.Merge().Subscribe(observer);
        }
    }
}