using System;
using System.Reactive.Subjects;
using Windows.System;
using Windows.UI.Core;
using AxelSmash.Smashes;

namespace AxelSmash.Providers
{
    public sealed class CoreWindowKeySmashProvider : ISmashProvider
    {
        private readonly Subject<IBabySmash> smashes = new Subject<IBabySmash>();
        private readonly CoreWindow window;

        public CoreWindowKeySmashProvider(CoreWindow window)
        {
            this.window = window;

            window.KeyDown += Window_KeyDown;
        }

        public void Dispose()
        {
            window.KeyDown -= Window_KeyDown;
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return smashes.Subscribe(observer);
        }

        private void Window_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey >= VirtualKey.GamepadA && args.VirtualKey <= VirtualKey.GamepadRightThumbstickLeft)
                return;

            var smash = new KeyboarSmash(args.VirtualKey);
            smashes.OnNext(smash);
        }
    }
}