using System;
using AxelSmash.Smashes;

namespace AxelSmash.Providers
{
    public interface ISmashProvider : IDisposable, IObservable<IBabySmash>
    {
    }
}