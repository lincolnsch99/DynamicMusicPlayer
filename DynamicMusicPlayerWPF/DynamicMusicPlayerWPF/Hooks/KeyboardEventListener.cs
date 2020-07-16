using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMusicPlayerWPF
{
    public interface KeyboardEventListener
    {
        void Update(KeyboardEvent e);
    }
}