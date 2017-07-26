using Windows.System;

namespace AxelSmash
{
    public class KeyboarSmash : IBabySmash
    {
        public VirtualKey Key { get; }

        public KeyboarSmash(VirtualKey key)
        {
            Key = key;
        }
    }
}