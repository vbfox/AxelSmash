using System;
using AxelSmash.Smashes;

namespace AxelSmash.Uwp.SmashSources
{
    public interface ISmashSource : IDisposable, IObservable<IBabySmash>
    {
    }
}