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

namespace DynamicMusicPlayerWPF.MusicUploadUI
{
    /// <summary>
    /// Interaction logic for AudioDisplay.xaml
    /// </summary>
    public partial class AudioDisplay : UserControl
    {
        private AudioDisplayModel model;
        
        public AudioDisplayModel Model { get => model; }

        public AudioDisplay()
        {
            InitializeComponent();
            model = new AudioDisplayModel(this);
            intensityCombobox.SelectionChanged += IntensityCombobox_SelectionChanged;
        }

        private void IntensityCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.ChangeIntensity(intensityCombobox.SelectedValue.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            model.RemoveAudioFromList();
        }
    }
}
