using Windows.Gaming.Input;

namespace AxelSmash
{
    public class GamepadButtonSmash : IBabySmash
    {
        public GamepadButtons Button { get; }

        public GamepadButtonSmash(GamepadButtons button)
        {
            Button = button;
        }
    }
}