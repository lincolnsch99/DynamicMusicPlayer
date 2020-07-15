using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicKeyloggerWPF.KeyboardUI
{
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

        public void Selected()
        {
            ChangeIntensity(KeyIntensity.UNIDENTIFIED);
            fullKeyboardReference.MainWindowReference.ApplyKeyboard();
        }
    }
}
