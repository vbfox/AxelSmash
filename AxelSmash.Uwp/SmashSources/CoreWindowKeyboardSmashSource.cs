using System;
using System.Reactive.Subjects;
using Windows.System;
using Windows.UI.Core;
using AxelSmash.Smashes;
using AxelSmash.SmashSources;
using AxelSmash.Uwp.Smashes;

namespace AxelSmash.Uwp.SmashSources
{
    public sealed class CoreWindowKeyboardSmashSource : ISmashSource
    {
        private readonly Subject<IBabySmash> smashes = new Subject<IBabySmash>();
        private readonly CoreWindow window;

        public CoreWindowKeyboardSmashSource(CoreWindow window)
        {
            this.window = window;

            window.KeyDown += WindowOnKeyDown;
        }

        public void Dispose()
        {
            window.KeyDown -= WindowOnKeyDown;

            smashes.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return smashes.Subscribe(observer);
        }

        private void WindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey >= VirtualKey.GamepadA && args.VirtualKey <= VirtualKey.GamepadRightThumbstickLeft)
                return;

            var smash = new KeyboardSmash(args.VirtualKey);
            smashes.OnNext(smash);
        }
    }
}