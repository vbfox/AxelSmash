using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using AxelSmash.Smashes;

namespace AxelSmash.Providers
{
    class CompositeSmashProvider : ISmashProvider
    {
        private ImmutableArray<ISmashProvider> providers;

        public CompositeSmashProvider(params ISmashProvider[] providers)
        {
            this.providers = providers.ToImmutableArray();
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