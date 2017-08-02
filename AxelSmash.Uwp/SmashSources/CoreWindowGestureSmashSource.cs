using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using AxelSmash.Smashes;
using AxelSmash.SmashSources;

namespace AxelSmash.Uwp.SmashSources
{
    public sealed class CoreWindowGestureSmashSource : ISmashSource
    {
        private readonly Subject<IBabySmash> smashes = new Subject<IBabySmash>();
        private readonly CoreWindow window;
        private readonly GestureRecognizer gestureRecognizer;

        public CoreWindowGestureSmashSource(CoreWindow window)
        {
            this.window = window;

            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.GestureSettings = GestureSettings.ManipulationScale |
                                                GestureSettings.ManipulationTranslateX |
                                                GestureSettings.ManipulationTranslateY |
                                                GestureSettings.ManipulationScale;
            gestureRecognizer.ManipulationStarted += GestureRecognizer_ManipulationStarted;
            gestureRecognizer.ManipulationUpdated += GestureRecognizerOnManipulationUpdated;
            gestureRecognizer.ManipulationCompleted += GestureRecognizerOnManipulationCompleted;

            window.PointerMoved += WindowOnPointerMoved;
            window.PointerWheelChanged += WindowOnPointerWheelChanged;
            window.PointerPressed += WindowOnPointerPressed;
            window.PointerReleased += WindowOnPointerReleased;
        }

        private void GestureRecognizerOnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            Debug.WriteLine("Completed {4}: Exp {0}, Rot {1}, Scale {2}, Trans {3}", args.Cumulative.Expansion, args.Cumulative.Rotation, args.Cumulative.Scale,
                args.Cumulative.Translation, args.PointerDeviceType);
        }

        private void GestureRecognizerOnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            Debug.WriteLine("Updated {4}: Exp {0}, Rot {1}, Scale {2}, Trans {3}", args.Cumulative.Expansion, args.Cumulative.Rotation, args.Cumulative.Scale,
                args.Cumulative.Translation, args.PointerDeviceType);
        }

        private void GestureRecognizer_ManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            Debug.WriteLine("Started {4}: Exp {0}, Rot {1}, Scale {2}, Trans {3}", args.Cumulative.Expansion, args.Cumulative.Rotation, args.Cumulative.Scale,
                args.Cumulative.Translation, args.PointerDeviceType);
        }

        private void WindowOnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessDownEvent(args.CurrentPoint);
        }

        private void WindowOnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessUpEvent(args.CurrentPoint);
        }

        private void WindowOnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints());
        }

        private void WindowOnPointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessMouseWheelEvent(
                args.CurrentPoint,
                (args.KeyModifiers & VirtualKeyModifiers.Shift) == VirtualKeyModifiers.Shift,
                (args.KeyModifiers & VirtualKeyModifiers.Control) == VirtualKeyModifiers.Control);
        }

        public void Dispose()
        {
            window.PointerReleased -= WindowOnPointerReleased;
            window.PointerPressed -= WindowOnPointerPressed;
            window.PointerWheelChanged -= WindowOnPointerWheelChanged;
            window.PointerMoved -= WindowOnPointerMoved;

            gestureRecognizer.ManipulationStarted -= GestureRecognizer_ManipulationStarted;

            smashes.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return smashes.Subscribe(observer);
        }
    }
}