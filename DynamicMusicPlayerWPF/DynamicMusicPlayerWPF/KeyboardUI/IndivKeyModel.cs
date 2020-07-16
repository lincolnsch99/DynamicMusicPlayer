using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DynamicMusicPlayerWPF.KeyboardUI
{
    /// <summary>
    /// Logic for the IndivKey UI element
    /// </summary>
    public class IndivKeyModel
    {
        private IndivKey view;
        private KeyIntensity intensity;
        private bool active;
        private FullKeyboardModel fullKeyboardReference;

        public FullKeyboardModel FullKeyboardReference { get => fullKeyboardReference; set => fullKeyboardReference = value; }
        public KeyIntensity Intensity { get => intensity; }

        public IndivKeyModel(IndivKey view)
        {
            this.view = view;
            intensity = KeyIntensity.NEUTRAL;
            SetActiveState(true);
        }

        /// <summary>
        /// Sets the active state of the key.
        /// </summary>
        /// <param name="set">True if active, False otherwise.</param>
        public void SetActiveState(bool set)
        {
            active = set;
            if(!active)
            {
                view.selectButton.IsEnabled = false;
                view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(203, 203, 203));
                view.keyText.Foreground = new SolidColorBrush(Color.FromRgb(93, 93, 93));
            }
            else
            {
                view.selectButton.IsEnabled = true;
                ChangeIntensity(KeyIntensity.NEUTRAL);
                view.keyText.Foreground = new SolidColorBrush(Color.FromRgb(0,0,0));
            }
        }

        /// <summary>
        /// Changes the intensity. If given UNIDENTIFIED, it will cycle the intensity according to the preset path,
        /// otherwise it will change to the given intensity.
        /// </summary>
        /// <param name="keyIntensity">Desired intensity.</param>
        public void ChangeIntensity(KeyIntensity keyIntensity)
        {
            if (keyIntensity == KeyIntensity.UNIDENTIFIED)
            {
                if (intensity == KeyIntensity.NEUTRAL)
                {
                    intensity = KeyIntensity.POSITIVE;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(255, 96, 96));
                }
                else if (intensity == KeyIntensity.POSITIVE)
                {
                    intensity = KeyIntensity.NEGATIVE;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(96, 96, 255));
                }
                else
                {
                    intensity = KeyIntensity.NEUTRAL;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(203, 203, 203));
                }
            }
            else
            {
                if (keyIntensity == KeyIntensity.POSITIVE)
                {
                    intensity = KeyIntensity.POSITIVE;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(255, 96, 96));
                }
                else if (keyIntensity == KeyIntensity.NEGATIVE)
                {
                    intensity = KeyIntensity.NEGATIVE;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(96, 96, 255));
                }
                else
                {
                    intensity = KeyIntensity.NEUTRAL;
                    view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(203, 203, 203));
                }
            }
        }

        /// <summary>
        /// Cycles the intensity value when selected, notifying the main window as well.
        /// </summary>
        public void Selected()
        {
            ChangeIntensity(KeyIntensity.UNIDENTIFIED);
            fullKeyboardReference.MainWindowReference.ApplyKeyboard();
        }
    }
}
