using Windows.System;
using AxelSmash.Smashes;

namespace AxelSmash.Uwp.Smashes
{
    public class KeyboardSmash : IBabySmash
    {
        public KeyboardSmash(VirtualKey key)
        {
            Key = key;
            Letter = VirtualKeyToChar(key);
        }

        public VirtualKey Key { get; }
        public char? Letter { get; }

        private static char? VirtualKeyToChar(VirtualKey key)
        {
            if (key >= VirtualKey.A && key <= VirtualKey.Z)
            {
                var i = key - VirtualKey.A;
                return (char)('a' + i);
            }

            if (key >= VirtualKey.Number0 && key < VirtualKey.Number9)
            {
                var i = key - VirtualKey.Number0;
                return (char)('0' + i);
            }

            if (key >= VirtualKey.NumberPad0 && key < VirtualKey.NumberPad9)
            {
                var i = key - VirtualKey.NumberPad0;
                return (char)('0' + i);
            }

            return null;
        }
    }
}