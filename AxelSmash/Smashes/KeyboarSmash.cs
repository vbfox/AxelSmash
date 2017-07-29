using Windows.System;

namespace AxelSmash.Smashes
{
    public class KeyboarSmash : IBabySmash
    {
        public KeyboarSmash(VirtualKey key)
        {
            Key = key;
            
        }

        public VirtualKey Key { get; }
        public char? Letter => VirtualKeyToChar(Key);

        private static char? VirtualKeyToChar(VirtualKey key)
        {
            if (key >= VirtualKey.A && key <= VirtualKey.Z)
            {
                var i = key - VirtualKey.A;
                return (char)('a' + i);
            }

            return null;
        }
    }
}