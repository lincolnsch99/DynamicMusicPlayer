using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMusicPlayerWPF
{
    public enum KeyIntensity
    {
        NEGATIVE,
        NEUTRAL,
        POSITIVE,
        UNIDENTIFIED
    }

    /// <summary>
    /// Listens in on all keyboard events, updating the intensity value continuously.
    /// </summary>
    public class IntensityTracker : KeyboardEventListener
    {
        private static float DEFAULT_INTENSITY = 0;
        private static float DEFAULT_INTERVALSIZE = 3;
        private static int DEFAULT_NUMINTERVALS = 3;
        private static float DEFAULT_INTENSITYDECAY = 0.15f;
        private static float DEFAULT_TRIGGERVALUE = 0.3f;

        private float intensity;
        private float intervalSize;
        private int numIntervals;
        private float intensityDecay;
        private float triggerValue;
        private Dictionary<KeyboardHook.VKeys, KeyIntensity> keyboardIntensityValues;

        public float Intensity { get => intensity; }
        public float IntervalSize { get => intervalSize; }

        public IntensityTracker()
        {
            keyboardIntensityValues = new Dictionary<KeyboardHook.VKeys, KeyIntensity>();
            intensity = DEFAULT_INTENSITY;
            intervalSize = DEFAULT_INTERVALSIZE;
            numIntervals = DEFAULT_NUMINTERVALS;
            intensityDecay = DEFAULT_INTENSITYDECAY;
            triggerValue = DEFAULT_TRIGGERVALUE;
            InitializeKeyboardIntensityValues();
        }
        
        /// <summary>
        /// Called from the event bus, changes the intensity according to the key that was pressed.
        /// </summary>
        /// <param name="e">The keyboard event being broadcasted.</param>
        public void Update(KeyboardEvent e)
         {
            keyboardIntensityValues.TryGetValue(e.KeyPressed, out KeyIntensity value);
            intensity += KeyIntensityToFloat(value);
            if (intensity > intervalSize * numIntervals)
                intensity = intervalSize * numIntervals; // Making sure the intensity doesn't go too high.
        }

        /// <summary>
        /// Decay happens once per second, and cannot decay below zero.
        /// </summary>
        public void Decay()
        {
            intensity -= intensityDecay;
            if (intensity < 0)
                intensity = 0; // Making sure the intensity doesn't go too low.
        }

        /// <summary>
        /// Changes the interval size.
        /// </summary>
        /// <param name="set">The desired interval size.</param>
        public void SetIntervalSize(float set)
        {
            if (set > 0)
                intervalSize = set;
        }

        /// <summary>
        /// Changes the trigger value, automatically updating the decay value as well.
        /// </summary>
        /// <param name="set">The desired value for trigger value.</param>
        public void SetTriggerValue(float set)
        {
            triggerValue = set;
            SetDecayValue(set * 0.5f);
        }

        /// <summary>
        /// Changes the decay value.
        /// </summary>
        /// <param name="set">The desired decay value.</param>
        public void SetDecayValue(float set)
        {
            intensityDecay = set;
        }

        /// <summary>
        /// Initializes all keyboard values to neutral(0.0).
        /// </summary>
        private void InitializeKeyboardIntensityValues()
        {
            foreach (KeyboardHook.VKeys key in Enum.GetValues(typeof(KeyboardHook.VKeys)))
            {
                try
                {
                    keyboardIntensityValues.Add(key, KeyIntensity.NEUTRAL);
                }
                catch
                {
                    // Empty catch
                }
            }
        }

        /// <summary>
        /// Registers the given key's intensity value.
        /// </summary>
        /// <param name="key">Key being registered.</param>
        /// <param name="intensity">Intensity value of the key.</param>
        public void RegisterKeyboardIntensityValue(KeyboardHook.VKeys key, KeyIntensity intensity)
        {
            keyboardIntensityValues[key] = intensity;
        }

        /// <summary>
        /// Resets the intensity value of the given key to NEUTRAL.
        /// </summary>
        /// <param name="key">Key being reset.</param>
        public void ResetKeyboardIntensityValue(KeyboardHook.VKeys key)
        {
            keyboardIntensityValues[key] = KeyIntensity.NEUTRAL;
        }

        /// <summary>
        /// Converts an intensity value to a float (according to the trigger value).
        /// </summary>
        /// <param name="key">Key intensity being converted.</param>
        /// <returns>Float cooresponding to the current trigger value and given intensity.</returns>
        private float KeyIntensityToFloat(KeyIntensity key)
        {
            float num;
            if (key == KeyIntensity.NEGATIVE)
                num = -triggerValue;
            else if (key == KeyIntensity.POSITIVE)
                num = triggerValue;
            else
                num = 0;
            return num;
        }
    }
}
