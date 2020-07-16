using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DynamicMusicPlayerWPF
{
    /// <summary>
    /// KeyRecorder acts as the event bus, creating keyboard events and notifying any listeners as the keys are 
    /// pressed. It is a singleton, there cannot be more than one bus.
    /// </summary>
    public class KeyRecorder : KeyboardEventBus
    {
        private KeyboardHook keyboardHook;
        private MouseHook mouseHook;
        private List<KeyboardEventListener> listeners;
        private KeyboardEvent curEvent;
        private static KeyRecorder instance;

        private KeyRecorder()
        {
            keyboardHook = new KeyboardHook();
            mouseHook = new MouseHook();
            mouseHook.LeftButtonDown += MouseHook_LeftButtonDown;
            mouseHook.RightButtonDown += MouseHook_RightButtonDown;
            mouseHook.MiddleButtonDown += MouseHook_MiddleButtonDown;
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
            listeners = new List<KeyboardEventListener>();
            curEvent = null;
        }

        /// <summary>
        /// Public access to the singleton instance.
        /// </summary>
        /// <returns></returns>
        public static KeyRecorder GetInstance()
        {
            if (instance == null)
                instance = new KeyRecorder();
            return instance;
        }

        /// <summary>
        /// Attached to occur whenever the middle mouse button is pressed.
        /// </summary>
        /// <param name="mouseStruct"></param>
        private void MouseHook_MiddleButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.MBUTTON);
            Notify();
        }

        /// <summary>
        /// Attached to occur whenever the right mouse button is pressed.
        /// </summary>
        /// <param name="mouseStruct"></param>
        private void MouseHook_RightButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.RBUTTON);
            Notify();
        }

        /// <summary>
        /// Attached to occur whenever the left mouse button is pressed.
        /// </summary>
        /// <param name="mouseStruct"></param>
        private void MouseHook_LeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.LBUTTON);
            Notify();
        }

        /// <summary>
        /// Attached to occur whenever a keyboard key is pressed.
        /// </summary>
        /// <param name="mouseStruct"></param>
        private void KeyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            curEvent = new KeyboardEvent(key);
            Notify();
        }

        /// <summary>
        /// Installs the mouse and keyboard hooks, enabling the program to observe any keyboard/mouse action.
        /// </summary>
        public void InstallHooks()
        {
            mouseHook.Install();
            keyboardHook.Install();
        }

        /// <summary>
        /// Uninstalls all hooks.
        /// </summary>
        public void UninstallHooks()
        {
            mouseHook.Uninstall();
            keyboardHook.Uninstall();
        }

        /// <summary>
        /// Notifies the listeners of the current event.
        /// </summary>
        public void Notify()
        {
            if (curEvent != null)
            {
                foreach (KeyboardEventListener listener in listeners)
                    listener.Update(curEvent);
                curEvent = null;
            }
        }

        /// <summary>
        /// Registers the given listenter.
        /// </summary>
        /// <param name="l"></param>
        public void RegisterListener(KeyboardEventListener l)
        {
            listeners.Add(l);
        }

        /// <summary>
        /// Unregisters the given listener.
        /// </summary>
        /// <param name="l"></param>
        public void UnregisterListener(KeyboardEventListener l)
        {
            listeners.Remove(l);
        }

        /// <summary>
        /// Handles uninstalling any possible keyboard or mouse hooks, this MUST be called when the application is 
        /// closed. If not, these hooks will stay active in the machine longer than desired. Automatically called
        /// when the application is closed.
        /// </summary>
        public void OnApplicationExit()
        {
            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
            mouseHook.RightButtonDown -= new MouseHook.MouseHookCallback(MouseHook_RightButtonDown);
            mouseHook.LeftButtonDown -= new MouseHook.MouseHookCallback(MouseHook_LeftButtonDown);
            UninstallHooks();
        }

        /// <summary>
        /// Only called when the application starts, installs the global hooks.
        /// </summary>
        public void OnApplicationStart()
        {
            InstallHooks();
        }
    }
}
