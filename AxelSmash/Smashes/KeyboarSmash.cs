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
    }
}