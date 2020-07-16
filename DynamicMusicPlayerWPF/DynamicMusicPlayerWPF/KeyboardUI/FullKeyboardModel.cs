using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace DynamicMusicPlayerWPF.KeyboardUI
{
    /// <summary>
    /// Logic for the FullKeyboard UI element.
    /// </summary>
    public class FullKeyboardModel
    {
        private FullKeyboard view;
        private MainWindowModel mainWindowReference;

        public MainWindowModel MainWindowReference { get => mainWindowReference; set => mainWindowReference = value; }

        public FullKeyboardModel(FullKeyboard view)
        {
            this.view = view;
        }

        /// <summary>
        /// Opens the file dialogue to allow for a user to load a preset, then applies the selected preset to the 
        /// keyboard.
        /// </summary>
        public void LoadPreset()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Keyboard Preset Files (*.kbp)|*.kbp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                KeyboardPreset preset = ReadFromBinaryFile(openFileDialog.FileName);
                foreach (Object obj in view.keyboardGrid.Children)
                {
                    if (obj.GetType() == typeof(IndivKey))
                    {
                        IndivKey key = (IndivKey)obj;
                        if (preset.NegativeIntensityKeys.Contains(key.keyText.Text))
                        {
                            key.Model.ChangeIntensity(KeyIntensity.NEGATIVE);
                        }
                        else if (preset.PositiveIntensityKeys.Contains(key.keyText.Text))
                        {
                            key.Model.ChangeIntensity(KeyIntensity.POSITIVE);
                        }
                        else
                        {
                            key.Model.ChangeIntensity(KeyIntensity.NEUTRAL);
                        }
                    }
                }
            }
            mainWindowReference.ApplyKeyboard();
        }

        /// <summary>
        /// Opens the file dialogue to allow for a user to save their preset, then creates/replaces a preset file.
        /// </summary>
        public void SavePreset()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Keyboard Preset Files (*.kbp)|*.kbp";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                KeyboardPreset preset = new KeyboardPreset();

                foreach (Object obj in view.keyboardGrid.Children)
                {
                    if (obj.GetType() == typeof(IndivKey))
                    {
                        IndivKey key = (IndivKey)obj;
                        if (key.Model.Intensity == KeyIntensity.NEGATIVE)
                            preset.NegativeIntensityKeys.Add(key.keyText.Text);
                        else if (key.Model.Intensity == KeyIntensity.POSITIVE)
                            preset.PositiveIntensityKeys.Add(key.keyText.Text);
                    }
                }

                WriteToBinaryFile(saveFileDialog.FileName, preset);
            }
        }

        /// <summary>
        /// Resets all keys to NEUTRAL, notifies the main window of the change.
        /// </summary>
        public void ResetKeyboard()
        {
            foreach (Object obj in view.keyboardGrid.Children)
            {
                if (obj.GetType() == typeof(IndivKey))
                {
                    IndivKey key = (IndivKey)obj;
                    key.Model.ChangeIntensity(KeyIntensity.NEUTRAL);
                }
            }
            mainWindowReference.ApplyKeyboard();
        }

        /// <summary>
        /// Writes the given object to a binary file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="objectToWrite"></param>
        /// <param name="append"></param>
        private void WriteToBinaryFile(string filePath, KeyboardPreset objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads from the given binary file, turning the data into a specific data type (KeyboardPreset in our case).
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private KeyboardPreset ReadFromBinaryFile(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (KeyboardPreset)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
