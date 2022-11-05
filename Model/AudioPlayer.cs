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

        private readonly MediaPlayer mediaPlayer;

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
            this.mediaPlayer = new MediaPlayer();
            this.mediaPlayer.AutoPlay = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the audio from the specified file.
        /// </summary>
        /// <param name="file">The audio file that will be played.</param>
        public void PlayAudio(StorageFile file)
        {
            this.mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            this.mediaPlayer.Play();
        }

        private static async Task<StorageFolder> loadFolder()
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            return folder;
        }

        #endregion
    }
}