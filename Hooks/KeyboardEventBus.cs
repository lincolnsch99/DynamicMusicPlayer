using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKeyloggerWPF
{
    public interface KeyboardEventBus
    {
        void Notify();
        void RegisterListener(KeyboardEventListener l);
        void UnregisterListener(KeyboardEventListener l);
    }
}
