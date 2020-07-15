﻿using GlobalLowLevelHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MusicKeyloggerWPF
{
    public enum SongIntensity
    {
        LOW,
        MEDIUM,
        HIGH
    }

    public class AudioController
    {
        private static float FADE_TIME = 3;
        private static float PAD_TIME = 7;

        private IntensityTracker intensityTracker; // Tracks decimal value of intensity.
        private SongIntensity intensityLevel; // Uses intensityTracker to determine what level of audio will be played.
        private DispatcherTimer dispatcherTimer;
        private MediaElement primaryPlayer;
        private MediaElement secondaryPlayer;
        private int tickCounter, tickSinceSwitch;

        private string curSongFilepath;
        private string nextSongFilepath;
        private int curMediaPlayer; // 1 if using the primary, 2 if secondary
        private double maxVolume;

        private List<string> lowIntensityFilepaths;
        private List<string> lowIntensityAlreadyPlayed;
        private List<string> medIntensityFilepaths;
        private List<string> medIntensityAlreadyPlayed;
        private List<string> highIntensityFilepaths;
        private List<string> highIntensityAlreadyPlayed;


        public AudioController(MediaElement primary, MediaElement secondary)
        {
            intensityTracker = new IntensityTracker();
            intensityLevel = SongIntensity.LOW;
            Keylogger.GetInstance().RegisterListener(intensityTracker);
            curSongFilepath = "";
            nextSongFilepath = "";

            lowIntensityFilepaths = new List<string>();
            lowIntensityAlreadyPlayed = new List<string>();
            medIntensityFilepaths = new List<string>();
            medIntensityAlreadyPlayed = new List<string>();
            highIntensityFilepaths = new List<string>();
            highIntensityAlreadyPlayed = new List<string>();

            primaryPlayer = primary;
            secondaryPlayer = secondary;
            primaryPlayer.LoadedBehavior = MediaState.Manual;
            secondaryPlayer.LoadedBehavior = MediaState.Manual;
            curMediaPlayer = 1;
            maxVolume = 1;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            tickCounter = 0;
            tickSinceSwitch = 5;
        }

        public void Reset()
        {
            StopPlaying();
            lowIntensityFilepaths = new List<string>();
            lowIntensityAlreadyPlayed = new List<string>();
            medIntensityFilepaths = new List<string>();
            medIntensityAlreadyPlayed = new List<string>();
            highIntensityFilepaths = new List<string>();
            highIntensityAlreadyPlayed = new List<string>();
            curMediaPlayer = 1;
            intensityLevel = SongIntensity.LOW;
            tickCounter = 0;
        }

        public void StartPlaying()
        {
            if (CheckCanPlay())
            {
                curSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                StartAudio(1, curSongFilepath);
                tickSinceSwitch = 0;
                dispatcherTimer.Start();
            }
        }

        public void StopPlaying()
        {
            StopAudio(curMediaPlayer);
            dispatcherTimer.Stop();
        }

        private bool CheckCanPlay()
        {
            return !(lowIntensityFilepaths.Count < 1 || medIntensityFilepaths.Count < 1 
                || highIntensityFilepaths.Count < 1);
        }

        public void RegisterAudio(string filepath, SongIntensity intensity)
        {
            if (intensity == SongIntensity.LOW)
                lowIntensityFilepaths.Add(filepath);
            else if (intensity == SongIntensity.MEDIUM)
                medIntensityFilepaths.Add(filepath);
            else
                highIntensityFilepaths.Add(filepath);
        }

        public void UnregisterAudio(string filepath)
        {
            if (lowIntensityFilepaths.Contains(filepath))
                lowIntensityFilepaths.Remove(filepath);
            else if (medIntensityFilepaths.Contains(filepath))
                medIntensityFilepaths.Remove(filepath);
            else if (highIntensityFilepaths.Contains(filepath))
                highIntensityFilepaths.Remove(filepath);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            tickCounter += 1;
            tickSinceSwitch += 1;
            intensityTracker.Decay();
            SongIntensity updatedIntensity = UpdateIntensity();
            if (updatedIntensity != intensityLevel)
            {
                if (tickSinceSwitch > PAD_TIME)
                {
                    // Grab a random song from the correct intensity level and begin process to play it. Then load the
                    // next song variable with another song with the same intensity.
                    if (curMediaPlayer == 1)
                    {
                        StopAudio(1);
                        nextSongFilepath = GrabRandomFilepathOfIntensity(updatedIntensity, curSongFilepath);
                        StartAudio(2, nextSongFilepath);
                        curSongFilepath = nextSongFilepath;
                        curMediaPlayer = 2;
                    }
                    else
                    {
                        StopAudio(2);
                        nextSongFilepath = GrabRandomFilepathOfIntensity(updatedIntensity, curSongFilepath);
                        StartAudio(1, nextSongFilepath);
                        curSongFilepath = nextSongFilepath;
                        curMediaPlayer = 1;
                    }
                    intensityLevel = updatedIntensity;
                    tickSinceSwitch = 0;
                }
            }
            else
            {
                UpdateCurrentlyPlaying();
            }
        }

        public void SkipSong()
        {
            if (curMediaPlayer == 1)
            {
                StopAudio(1);
                nextSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                StartAudio(2, nextSongFilepath);
                curSongFilepath = nextSongFilepath;
                curMediaPlayer = 2;
            }
            else
            {
                StopAudio(2);
                nextSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                StartAudio(1, nextSongFilepath);
                curSongFilepath = nextSongFilepath;
                nextSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                curMediaPlayer = 1;
            }
        }

        /// <summary>
        /// Starts playing the audio from the given filepath, using the media player associated with the given int. The
        /// audio will automatically be faded in over the fade in time (DEFAULT 3 seconds).
        /// </summary>
        /// <param name="play">1 for primary player, 2 for secondary.</param>
        /// <param name="filePath">Filepath for the desired audio.</param>
        private void StartAudio(int play, string filePath)
        {
            // Both options should have their audio fade in for 3 seconds.
            if (play == 1)
            {
                // Start playing requested audio on the primary.
                primaryPlayer.Source = new Uri(filePath);
                primaryPlayer.Volume = 0;
                primaryPlayer.Play();
                primaryPlayer.Position = TimeSpan.FromSeconds(15);
                primaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(primaryPlayer.Volume, 
                    maxVolume, TimeSpan.FromSeconds(FADE_TIME)));
            }
            else
            {
                // Start playing requested audio on the secondary.
                secondaryPlayer.Source = new Uri(filePath);
                secondaryPlayer.Volume = 0;
                secondaryPlayer.Play();
                secondaryPlayer.Position = TimeSpan.FromSeconds(15);
                secondaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(secondaryPlayer.Volume,
                    maxVolume, TimeSpan.FromSeconds(FADE_TIME)));
            }
        }

        /// <summary>
        /// Stops playing the audio from the media player associated with the given int. The audio will be faded out
        /// over the fade out time (DEFAULT 3 seconds).
        /// </summary>
        /// <param name="stop">1 for primary player, 2 for secondary.</param>
        private void StopAudio(int stop)
        {
            // Both options should have their audio fade out for 3 seconds.
            if (stop == 1)
            {
                primaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(primaryPlayer.Volume, 0,
                    TimeSpan.FromSeconds(FADE_TIME)));
            }
            else
            {
                secondaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(secondaryPlayer.Volume,
                    0, TimeSpan.FromSeconds(FADE_TIME)));
            }
        }

        /// <summary>
        /// Searches through the correct group of filepaths and returns a random one that is different from the current
        /// song being played.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string GrabRandomFilepathOfIntensity(SongIntensity i, string curSong)
        {
            Random randGen = new Random();
            string filepath = "";
            if (i == SongIntensity.LOW)
            {
                if (lowIntensityFilepaths.Count > 0)
                {
                    while (filepath.Equals("") || filepath.Equals(curSong) || lowIntensityAlreadyPlayed.Contains(filepath))
                    {
                        int randNum = randGen.Next(0, lowIntensityFilepaths.Count);
                        filepath = lowIntensityFilepaths[randNum];
                    }
                    lowIntensityFilepaths.Remove(filepath);
                    lowIntensityAlreadyPlayed.Add(filepath);
                }
                else if (lowIntensityAlreadyPlayed.Count > 0)
                {
                    foreach (string fpath in lowIntensityAlreadyPlayed)
                        lowIntensityFilepaths.Add(fpath);
                    lowIntensityAlreadyPlayed.Clear();
                    filepath = GrabRandomFilepathOfIntensity(i, curSong);
                }
                else
                    filepath = curSong;
            }
            else if (i == SongIntensity.MEDIUM)
            {
                if (medIntensityFilepaths.Count > 0)
                {
                    while (filepath.Equals("") || filepath.Equals(curSong) || medIntensityAlreadyPlayed.Contains(filepath))
                    {
                        int randNum = randGen.Next(0, medIntensityFilepaths.Count);
                        filepath = medIntensityFilepaths[randNum];
                    }
                    medIntensityFilepaths.Remove(filepath);
                    medIntensityAlreadyPlayed.Add(filepath);
                }
                else if (medIntensityAlreadyPlayed.Count > 0)
                {
                    foreach (string fpath in medIntensityAlreadyPlayed)
                        medIntensityFilepaths.Add(fpath);
                    medIntensityAlreadyPlayed.Clear();
                    filepath = GrabRandomFilepathOfIntensity(i, curSong);
                }
                else
                    filepath = curSong;
            }
            else
            {
                if (highIntensityFilepaths.Count > 0)
                {
                    while (filepath.Equals("") || filepath.Equals(curSong) || highIntensityAlreadyPlayed.Contains(filepath))
                    {
                        int randNum = randGen.Next(0, highIntensityFilepaths.Count);
                        filepath = highIntensityFilepaths[randNum];
                    }
                    highIntensityFilepaths.Remove(filepath);
                    highIntensityAlreadyPlayed.Add(filepath);
                }
                else if (highIntensityAlreadyPlayed.Count > 0)
                {
                    foreach (string fpath in highIntensityAlreadyPlayed)
                        highIntensityFilepaths.Add(fpath);
                    highIntensityAlreadyPlayed.Clear();
                    filepath = GrabRandomFilepathOfIntensity(i, curSong);
                }
                else
                    filepath = curSong;
            }
            return filepath;
        }

        private SongIntensity UpdateIntensity()
        {
            SongIntensity _intensityLevel;
            if (intensityTracker.Intensity < intensityTracker.IntervalSize)
                _intensityLevel = SongIntensity.LOW;
            else if (intensityTracker.Intensity < intensityTracker.IntervalSize * 2)
                _intensityLevel = SongIntensity.MEDIUM;
            else
                _intensityLevel = SongIntensity.HIGH;
            return _intensityLevel;
        }

        private void UpdateCurrentlyPlaying()
        {
            if (curMediaPlayer == 1)
            {
                if (primaryPlayer.NaturalDuration.HasTimeSpan && 
                    primaryPlayer.Position.TotalSeconds >= primaryPlayer.NaturalDuration.TimeSpan.TotalSeconds - 15)
                {
                    // Start playing next song, then load the next song variable with another song with the same 
                    // intensity.
                    StopAudio(1);
                    nextSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                    StartAudio(2, nextSongFilepath);
                    curSongFilepath = nextSongFilepath;
                    curMediaPlayer = 2;
                }
            }
            else
            {
                if (secondaryPlayer.NaturalDuration.HasTimeSpan && 
                    secondaryPlayer.Position.TotalSeconds >= secondaryPlayer.NaturalDuration.TimeSpan.TotalSeconds - 15)
                {
                    // Start playing next song, then load the next song variable with another song with the same 
                    // intensity.
                    StopAudio(2);
                    nextSongFilepath = GrabRandomFilepathOfIntensity(intensityLevel, curSongFilepath);
                    StartAudio(1, nextSongFilepath);
                    curSongFilepath = nextSongFilepath;
                    curMediaPlayer = 1;
                }
            }
        }

        public void RegisterKeyIntensityValue(KeyboardHook.VKeys key, KeyIntensity intensity)
        {
            intensityTracker.RegisterKeyboardIntensityValue(key, intensity);
        }

        private void ResetKeyIntensityValue(KeyboardHook.VKeys key)
        {
            intensityTracker.ResetKeyboardIntensityValue(key);
        }

        public void AdjustMaxVolume(double value)
        {
            maxVolume = value;
            if (curMediaPlayer == 1)
                primaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(primaryPlayer.Volume,
                    maxVolume, TimeSpan.FromSeconds(0.1)));
            else
                secondaryPlayer.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(secondaryPlayer.Volume,
                    maxVolume, TimeSpan.FromSeconds(0.1)));
        }

        public void AdjustSensitivity(float value)
        {
            intensityTracker.SetTriggerValue(value);
        }

    }
}
