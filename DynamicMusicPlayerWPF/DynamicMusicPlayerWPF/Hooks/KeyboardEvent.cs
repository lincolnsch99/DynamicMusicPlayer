using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMusicPlayerWPF
{
    /// <summary>
    /// KeyboardEvents are used in the event bus to notify listeners of any keyboard event that occurs.
    /// </summary>
    public class KeyboardEvent
    {
        private KeyboardHook.VKeys keyPressed;
        public KeyboardHook.VKeys KeyPressed { get => keyPressed; set => keyPressed = value; }

        public KeyboardEvent(KeyboardHook.VKeys keyPressed)
        {
            this.keyPressed = keyPressed;
        }
    }
}