﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicKeyloggerWPF.MusicUploadUI
{
    public class AudioDisplayModel
    {
        private AudioDisplay view;
        private SongIntensity intensity;
        private string title, filePath;
        private MainWindowModel mainWindowReference;

        public string FilePath { get => filePath; set => filePath = value; }
        public SongIntensity Intensity { get => intensity; }
        public MainWindowModel MainWindowReference { set => mainWindowReference = value; }

        public AudioDisplayModel(AudioDisplay view)
        {
            this.view = view;
        }

        public void ChangeIntensity(string intensity)
        {
            if(intensity == "Low")
            {
                this.intensity = SongIntensity.LOW;
                view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(156, 255, 165));
                view.intensityCombobox.SelectedItem = view.intensityCombobox.Items[0];
            }
            else if(intensity == "Medium")
            {
                this.intensity = SongIntensity.MEDIUM;
                view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 38));
                view.intensityCombobox.SelectedItem = view.intensityCombobox.Items[1];
            }
            else
            {
                this.intensity = SongIntensity.HIGH;
                view.backgroundColor.Fill = new SolidColorBrush(Color.FromRgb(255, 151, 151));
                view.intensityCombobox.SelectedItem = view.intensityCombobox.Items[2];
            }
            mainWindowReference.ApplyPlaylist();
        }

        public void SetAudio(string audioFilePath)
        {
            title = Path.GetFileName(audioFilePath);
            filePath = audioFilePath;
            view.TitleLabel.Content = title;
            intensity = SongIntensity.LOW;
            view.intensityCombobox.SelectedItem = view.intensityCombobox.Items[0];
        }

        public void RemoveAudioFromList()
        {
            mainWindowReference.RemoveAudio(this.view);
        }
    }
}
