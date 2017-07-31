using System;
using AxelSmash.Smashes;

namespace AxelSmash.SmashSources
{
    class CompositeSmashSource : ISmashSource
    {
        private ImmutableArray<ISmashSource> sources;

        public CompositeSmashSource(ImmutableArray<ISmashSource> sources)
        {
            this.sources = sources;
        }

        public void Dispose()
        {
            foreach (var provider in sources)
                provider.Dispose();
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return sources.Merge().Subscribe(observer);
        }
    }
}