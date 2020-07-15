using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicKeyloggerWPF
{
    public class Keylogger : KeyboardEventBus
    {
        private KeyboardHook keyboardHook;
        private MouseHook mouseHook;
        private List<KeyboardEventListener> listeners;
        private KeyboardEvent curEvent;
        private static Keylogger instance;

        private Keylogger()
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

        public static Keylogger GetInstance()
        {
            if (instance == null)
                instance = new Keylogger();
            return instance;
        }

        private void MouseHook_MiddleButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.MBUTTON);
            Notify();
        }

        private void MouseHook_RightButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.RBUTTON);
            Notify();
        }

        private void MouseHook_LeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            curEvent = new KeyboardEvent(KeyboardHook.VKeys.LBUTTON);
            Notify();
        }

        private void KeyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            curEvent = new KeyboardEvent(key);
            Notify();
        }

        public void InstallHooks()
        {
            mouseHook.Install();
            keyboardHook.Install();
        }

        public void UninstallHooks()
        {
            mouseHook.Uninstall();
            keyboardHook.Uninstall();
        }

        public void Notify()
        {
            if (curEvent != null)
            {
                foreach (KeyboardEventListener listener in listeners)
                    listener.Update(curEvent);
                curEvent = null;
            }
        }

        public void RegisterListener(KeyboardEventListener l)
        {
            listeners.Add(l);
        }

        public void UnregisterListener(KeyboardEventListener l)
        {
            listeners.Remove(l);
        }

        /// <summary>
        /// Handles uninstalling any possible keyboard or mouse hooks, this MUST be called when the application is 
        /// closed. If not, these hooks will stay active in the machine longer than desired.
        /// </summary>
        public void OnApplicationExit()
        {
            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
            mouseHook.RightButtonDown -= new MouseHook.MouseHookCallback(MouseHook_RightButtonDown);
            mouseHook.LeftButtonDown -= new MouseHook.MouseHookCallback(MouseHook_LeftButtonDown);
            UninstallHooks();
        }

        public void OnApplicationStart()
        {
            InstallHooks();
        }
    }
}
