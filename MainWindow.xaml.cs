using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicKeyloggerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel model;
        private AudioController audioController;

        public AudioController AudioController { get => audioController; }

        public MainWindow()
        {
            InitializeComponent();
            model = new MainWindowModel(this);
            audioController = new AudioController(primaryPlayer, secondaryPlayer);
            fullKeyboardView.Model.MainWindowReference = this.model;
            volumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            sensitivitySlider.ValueChanged += SensitivitySlider_ValueChanged;
            audioController.AdjustSensitivity((float)sensitivitySlider.Value);
        }

        private void SensitivitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioController.AdjustSensitivity((float)sensitivitySlider.Value);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioController.AdjustMaxVolume(volumeSlider.Value);
        }

        private void uploadSongButton_Click(object sender, RoutedEventArgs e)
        {
            model.UploadAudio();
        }

        private void savePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            model.SavePlaylist();
        }

        private void loadPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            model.LoadPlaylist();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            audioController.StartPlaying();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            audioController.StopPlaying();
        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            audioController.SkipSong();
        }

        private void clearPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            audioController.Reset();
            SongList.Items.Clear();
        }
    }
}
