using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace DodgeDots.Model
{
    /// <summary>
    ///     The Audio Player class.
    /// </summary>
    public class AudioPlayer
    {
        #region Data members

        private readonly MediaPlayer mediaPlayer = new MediaPlayer();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the audio folder.
        /// </summary>
        /// <value>
        ///     The audio folder.
        /// </value>
        public Task<StorageFolder> AudioFolder { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioPlayer" /> class.
        /// </summary>
        public AudioPlayer()
        {
            this.AudioFolder = loadFolder();
            this.mediaPlayer.AutoPlay = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the audio from the specified file, under a newly created instance of the class.
        ///     Prevents old audio playing from being terminated when new audio plays.
        ///     Precondition: none
        ///     Post-condition: new instance of class is created and then removed after 2 seconds
        /// </summary>
        /// <param name="file">The audio file that will be played.</param>
        public async void PlayAudio(StorageFile file)
        {
            const int audioDuration = 2;
            var tempAudioPlayer = new AudioPlayer
            {
                mediaPlayer =
                {
                    Source = MediaSource.CreateFromStorageFile(file)
                }
            };
            tempAudioPlayer.mediaPlayer.Play();
            await Task.Delay(audioDuration * 1000);
            removeTempAudio(out tempAudioPlayer);
        }

        private static async Task<StorageFolder> loadFolder()
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            return folder;
        }

        private static void removeTempAudio(out AudioPlayer tempAudio)
        {
            tempAudio = null;
        }

        #endregion
    }
}