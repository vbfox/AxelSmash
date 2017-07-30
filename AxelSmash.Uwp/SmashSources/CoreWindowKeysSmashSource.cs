using System;
using System.Reactive.Subjects;
using Windows.System;
using Windows.UI.Core;
using AxelSmash.Smashes;

namespace AxelSmash.SmashSources
{
    public sealed class CoreWindowKeysSmashSource : ISmashSource
    {
        private readonly Subject<IBabySmash> smashes = new Subject<IBabySmash>();
        private readonly CoreWindow window;

        public CoreWindowKeysSmashSource(CoreWindow window)
        {
            this.window = window;

            window.KeyDown += Window_KeyDown;
        }

        public void Dispose()
        {
            window.KeyDown -= Window_KeyDown;

            smashes.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return smashes.Subscribe(observer);
        }

        private void Window_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey >= VirtualKey.GamepadA && args.VirtualKey <= VirtualKey.GamepadRightThumbstickLeft)
                return;

            var smash = new KeyboardSmash(args.VirtualKey);
            smashes.OnNext(smash);
        }
    }
}