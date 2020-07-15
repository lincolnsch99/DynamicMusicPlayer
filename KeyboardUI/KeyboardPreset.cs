﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKeyloggerWPF.KeyboardUI
{
    [Serializable]
    public class KeyboardPreset
    {
        private List<String> negativeIntensityKeys;
        private List<String> positiveIntensityKeys;

        public List<String> NegativeIntensityKeys { get => negativeIntensityKeys; set => negativeIntensityKeys = value; }
        public List<String> PositiveIntensityKeys { get => positiveIntensityKeys; set => positiveIntensityKeys = value; }

        public KeyboardPreset()
        {
            negativeIntensityKeys = new List<string>();
            positiveIntensityKeys = new List<string>();
        }
    }
}
