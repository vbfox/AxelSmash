using System;
using AxelSmash.Smashes;

namespace AxelSmash.SmashSources
{
    public interface ISmashSource : IDisposable, IObservable<IBabySmash>
    {
    }
}