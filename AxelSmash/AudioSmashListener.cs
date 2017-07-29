using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxelSmash.Smashes;

namespace AxelSmash
{
    class AudioSmashListener : IObserver<IBabySmash>, IDisposable
    {
        public void OnCompleted()
        {
            Dispose();
        }

        public void OnError(Exception error)
        {
            Dispose();
        }

        public void OnNext(IBabySmash value)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
