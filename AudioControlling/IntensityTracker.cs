using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKeyloggerWPF
{
    public enum KeyIntensity
    {
        NEGATIVE,
        NEUTRAL,
        POSITIVE,
        UNIDENTIFIED
    }

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

        public void Update(KeyboardEvent e)
         {
            keyboardIntensityValues.TryGetValue(e.KeyPressed, out KeyIntensity value);
            intensity += KeyIntensityToFloat(value);
            if (intensity > intervalSize * numIntervals)
                intensity = intervalSize * numIntervals; // Making sure the intensity doesn't go too high.
        }

        public void Decay()
        {
            intensity -= intensityDecay;
            if (intensity < 0)
                intensity = 0; // Making sure the intensity doesn't go too low.
        }

        public void SetIntervalSize(float set)
        {
            if (set > 0)
                intervalSize = set;
        }

        public void SetTriggerValue(float set)
        {
            triggerValue = set;
            intensityDecay = set * 0.5f;
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

        public void RegisterKeyboardIntensityValue(KeyboardHook.VKeys key, KeyIntensity intensity)
        {
            keyboardIntensityValues[key] = intensity;
        }

        public void ResetKeyboardIntensityValue(KeyboardHook.VKeys key)
        {
            keyboardIntensityValues[key] = KeyIntensity.NEUTRAL;
        }

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
