using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace AxelSmash
{
    public class ControllerPolling : IDisposable
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(50);

        private class ControllerInfo
        {
            public RawGameController Raw { get; }
            public Gamepad Gamepad { get; }
            public GamepadButtons ButtonsPressed { get; set; } = GamepadButtons.None;

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
                return (Raw != null ? Raw.GetHashCode() : 0);
            }
        }

        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private ImmutableHashSet<ControllerInfo> controllers = ImmutableHashSet<ControllerInfo>.Empty;

        private static readonly ImmutableArray<GamepadButtons> AllButtons = GetAllButtons();

        private static ImmutableArray<GamepadButtons> GetAllButtons()
        {
            return Enum.GetValues(typeof(GamepadButtons))
                .Cast<GamepadButtons>()
                .Where(b => b != GamepadButtons.None)
                .ToImmutableArray();
        }
    
        public ControllerPolling()
        {
            Attach();
            Task.Factory.StartNew(PoolingLoop, TaskCreationOptions.LongRunning);
        }

        private void Attach()
        {
            RawGameController.RawGameControllerAdded += OnAdded;
            RawGameController.RawGameControllerRemoved += OnRemoved;
            
            foreach (var gamePad in RawGameController.RawGameControllers)
            {
                OnAdded(this, gamePad);
            }
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
                {
                    return;
                }
            
                foreach (var controller in controllers)
                {
                    if (controller.Gamepad != null)
                    {
                        ReadGamepadButtons(controller);
                    }
                }

                await Task.Delay(Interval);
            }
        }

        private void ReadGamepadButtons(ControllerInfo controller)
        {
            var previous = controller.ButtonsPressed;

            var reading = controller.Gamepad.GetCurrentReading();
            var current = reading.Buttons;
            controller.ButtonsPressed = current;
            
            var newPresses = current & ~previous;
            Debug.WriteLine($"{previous} -> {current} = {newPresses}");

            foreach (var potentialButton in AllButtons)
            {
                if ((potentialButton & newPresses) == potentialButton)
                {
                    Debug.WriteLine($"!!! {potentialButton}");
                    OnSmash(new GamepadButtonSmash(potentialButton));
                }
            }
        }

        public event Action<IBabySmash> Smash;

        public void Dispose()
        {
            cancellation.Cancel();

            RawGameController.RawGameControllerAdded -= OnAdded;
            RawGameController.RawGameControllerRemoved -= OnRemoved;
        }

        protected void OnSmash(IBabySmash obj)
        {
            Smash?.Invoke(obj);
        }
    }
}