using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMusicPlayerWPF
{
    /// <summary>
    /// Playlist is used to store audio filepaths to the users computer.
    /// </summary>
    [Serializable]
    public class Playlist
    {
        private List<string> lowIntensityFilepaths;
        private List<string> medIntensityFilepaths;
        private List<string> highIntensityFilepaths;

        public List<string> LowIntensityFilepaths { get => lowIntensityFilepaths; set => lowIntensityFilepaths = value; }
        public List<string> MedIntensityFilepaths { get => medIntensityFilepaths; set => medIntensityFilepaths = value; }
        public List<string> HighIntensityFilepaths { get => highIntensityFilepaths; set => highIntensityFilepaths = value; }

        public Playlist()
        {
            lowIntensityFilepaths = new List<string>();
            medIntensityFilepaths = new List<string>();
            highIntensityFilepaths = new List<string>();
        }
    }
}
