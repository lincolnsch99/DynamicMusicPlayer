using GlobalLowLevelHooks;
using MusicKeyloggerWPF.KeyboardUI;
using MusicKeyloggerWPF.MusicUploadUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicKeyloggerWPF
{
    public class MainWindowModel
    {
        private MainWindow view;

        public MainWindowModel(MainWindow view)
        {
            this.view = view;
        }

        public void UploadAudio()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio Files (*.mp3)|*.mp3|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    AudioDisplay song = new AudioDisplay();
                    song.Model.MainWindowReference = this;
                    song.Model.SetAudio(filename);
                    view.SongList.Items.Add(song);
                }
            }
            ApplyPlaylist();
        }

        public void LoadPlaylist()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Playlist Files (*.plst)|*.plst";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Playlist playlist = ReadFromBinaryFile(openFileDialog.FileName);
                foreach (string filename in playlist.LowIntensityFilepaths)
                {
                    AudioDisplay song = new AudioDisplay();
                    song.Model.MainWindowReference = this;
                    song.Model.SetAudio(filename);
                    song.Model.ChangeIntensity("Low");
                    view.SongList.Items.Add(song);
                }
                foreach (string filename in playlist.MedIntensityFilepaths)
                {
                    AudioDisplay song = new AudioDisplay();
                    song.Model.MainWindowReference = this;
                    song.Model.SetAudio(filename);
                    song.Model.ChangeIntensity("Medium");
                    view.SongList.Items.Add(song);
                }
                foreach (string filename in playlist.HighIntensityFilepaths)
                {
                    AudioDisplay song = new AudioDisplay();
                    song.Model.MainWindowReference = this;
                    song.Model.SetAudio(filename);
                    song.Model.ChangeIntensity("High");
                    view.SongList.Items.Add(song);
                }
            }
        }

        public void SavePlaylist()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Playlist Files (*.plst)|*.plst";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Playlist playlist = new Playlist();

                foreach (Object obj in view.SongList.Items)
                {
                    if (obj.GetType() == typeof(AudioDisplay))
                    {
                        AudioDisplay audio = (AudioDisplay)obj;
                        if (audio.Model.Intensity == SongIntensity.LOW)
                            playlist.LowIntensityFilepaths.Add(audio.Model.FilePath);
                        else if (audio.Model.Intensity == SongIntensity.MEDIUM)
                            playlist.MedIntensityFilepaths.Add(audio.Model.FilePath);
                        else
                            playlist.HighIntensityFilepaths.Add(audio.Model.FilePath);
                    }
                }

                WriteToBinaryFile(saveFileDialog.FileName, playlist);
            }
        }

        public void RemoveAudio(AudioDisplay audioDisplay)
        {
            view.SongList.Items.Remove(audioDisplay);
            view.AudioController.UnregisterAudio(audioDisplay.Model.FilePath);
            ApplyPlaylist();
        }

        public void ApplyPlaylist()
        {
            view.AudioController.Reset();
            foreach (AudioDisplay display in view.SongList.Items)
            {
                view.AudioController.RegisterAudio(display.Model.FilePath, display.Model.Intensity);
            }
        }

        public void ApplyKeyboard()
        {
            foreach(Object obj in view.fullKeyboardView.keyboardGrid.Children)
            {
                if (obj.GetType() == typeof(IndivKey))
                {
                    IndivKey key = (IndivKey)obj;
                    view.AudioController.RegisterKeyIntensityValue(KeyboardHook.StringToVKey(key.keyText.Text),
                        key.Model.Intensity);
                }
            }
        }

        private void WriteToBinaryFile(string filePath, Playlist objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        private Playlist ReadFromBinaryFile(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Playlist)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
