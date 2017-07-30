using Windows.Gaming.Input;

namespace AxelSmash.Smashes
{
    public class GamepadButtonSmash : IBabySmash
    {
        public GamepadButtonSmash(GamepadButtons button)
        {
            Button = button;
        }

        public GamepadButtons Button { get; }
        public char? Letter => null;
    }
}