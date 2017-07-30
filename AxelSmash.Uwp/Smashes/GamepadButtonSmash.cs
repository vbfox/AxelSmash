using Windows.Gaming.Input;
using AxelSmash.Smashes;

namespace AxelSmash.Uwp.Smashes
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