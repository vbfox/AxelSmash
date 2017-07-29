using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using AxelSmash.Smashes;

namespace AxelSmash.Providers
{
    public class ControllerSmashProvider : ISmashProvider
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(50);

        private static readonly ImmutableArray<GamepadButtons> AllButtons = GetAllButtons();

        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();

        private readonly Subject<IBabySmash> smashes = new Subject<IBabySmash>();
        private ImmutableHashSet<ControllerInfo> controllers = ImmutableHashSet<ControllerInfo>.Empty;

        public ControllerSmashProvider()
        {
            Attach();
            Task.Factory.StartNew(PoolingLoop, TaskCreationOptions.LongRunning);
        }

        public IObservable<IBabySmash> Smashes => smashes;

        public void Dispose()
        {
            cancellation.Cancel();

            RawGameController.RawGameControllerAdded -= OnAdded;
            RawGameController.RawGameControllerRemoved -= OnRemoved;
        }

        public IDisposable Subscribe(IObserver<IBabySmash> observer)
        {
            return smashes.Subscribe(observer);
        }

        private static ImmutableArray<GamepadButtons> GetAllButtons()
        {
            return Enum.GetValues(typeof(GamepadButtons))
                .Cast<GamepadButtons>()
                .Where(b => b != GamepadButtons.None)
                .ToImmutableArray();
        }

        private void Attach()
        {
            RawGameController.RawGameControllerAdded += OnAdded;
            RawGameController.RawGameControllerRemoved += OnRemoved;

            foreach (var gamePad in RawGameController.RawGameControllers)
                OnAdded(this, gamePad);
        }

        private void OnRemoved(object sender, RawGameController e)
        {
            var info = new ControllerInfo(e);
            controllers = controllers.Remove(info);
        }

        private void OnAdded(object sender, RawGameController e)
        {
            var info = new ControllerInfo(e, Gamepad.FromGameController(e));
            controllers = controllers.Add(info);
        }

        public async Task PoolingLoop()
        {
            while (true)
            {
                if (cancellation.Token.IsCancellationRequested)
                    return;

                foreach (var controller in controllers)
                    if (controller.Gamepad != null)
                        ReadGamepadButtons(controller);

                await Task.Delay(Interval);
            }
        }

        private void ReadGamepadButtons(ControllerInfo controller)
        {
            var previous = controller.ButtonsPressed;

            var reading = controller.Gamepad.GetCurrentReading();
            var current = reading.Buttons;
            controller.ButtonsPressed = current;

            if (smashes.HasObservers)
            {
                var newPresses = current & ~previous;

                foreach (var potentialButton in AllButtons)
                    if ((potentialButton & newPresses) == potentialButton)
                    {
                        var smash = new GamepadButtonSmash(potentialButton);
                        smashes.OnNext(smash);
                    }
            }
        }

        private class ControllerInfo
        {
            public ControllerInfo(RawGameController raw)
            {
                Raw = raw;
                Gamepad = null;
            }

            public ControllerInfo(RawGameController raw, Gamepad gamepad)
            {
                Raw = raw;
                Gamepad = gamepad;
            }

            public RawGameController Raw { get; }
            public Gamepad Gamepad { get; }
            public GamepadButtons ButtonsPressed { get; set; } = GamepadButtons.None;

            public bool Equals(ControllerInfo other)
            {
                return Equals(Raw, other.Raw);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is ControllerInfo && Equals((ControllerInfo) obj);
            }

            public override int GetHashCode()
            {
                return Raw != null ? Raw.GetHashCode() : 0;
            }
        }
    }
}