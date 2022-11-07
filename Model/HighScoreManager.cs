using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace DodgeDots.Model
{
    // TODO Is it ok to declare them like this?
    /// <summary>
    ///     Handles saving and loading of the score board.
    /// </summary>
    public class HighScoreManager
    {
        #region Data members

        private const string FilenameDataContractSerialization = "highscoresDataContract.xml";

        private Collection<UserScore> allScores;

        #endregion

        #region Properties

        /// <summary>
        ///     The top ten scores sorted.
        /// </summary>
        public Collection<UserScore> TopTen { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Loads the scoreboard from the saved file if it is present.
        /// </summary>
        /// <returns>An asynchronous operation.</returns>
        public async Task LoadHighScoresAsync()
        {
            var theFolder = ApplicationData.Current.LocalFolder;

            try
            {
                var theFile = await theFolder.GetFileAsync(FilenameDataContractSerialization);
                var inStream = await theFile.OpenStreamForReadAsync();

                var deserializer = new DataContractSerializer(typeof(Collection<UserScore>));
                this.allScores = (Collection<UserScore>)deserializer.ReadObject(inStream);

                inStream.Dispose();
            }
            catch (FileNotFoundException)
            {
                this.allScores = new Collection<UserScore>();
            }
        }

        /// <summary>
        ///     Saves the score board out to the file.
        /// </summary>
        /// <returns>An asynchronous operation.</returns>
        public async Task SaveHighScoresAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(FilenameDataContractSerialization,
                CreationCollisionOption.ReplaceExisting);
            var outStream = await file.OpenStreamForWriteAsync();

            var serializer = new DataContractSerializer(typeof(Collection<UserScore>));

            using (outStream)
            {
                serializer.WriteObject(outStream, this.allScores);
            }

            outStream.Dispose();
        }

        public async Task ResetHighScoresAsync()
        {
            this.allScores = new Collection<UserScore>();

            await this.SaveHighScoresAsync();
        }

        /// <summary>
        ///     Adds an entry to the scoreboard.
        /// </summary>
        /// <param name="score">The score achieved.</param>
        /// <param name="level">The level achieved.</param>
        /// <param name="name">The users name.</param>
        public void AddScore(int score, int level, string name)
        {
            this.allScores.Add(new UserScore(score, level, name));
        }

        /// <summary>
        ///     Sorts the top ten elements on the scoreboard.
        /// </summary>
        public void SortTopTen()
        {
            var order = this.allScores.OrderByDescending(x => x.Score).Take(10).ToArray();
            this.TopTen = new Collection<UserScore>(order);
        }

        #endregion
    }

    /// <summary>
    ///     Defines an entry in the scoreboard.
    /// </summary>
    [DataContract]
    public class UserScore
    {
        #region Properties

        /// <summary>
        ///     The score achieved.
        /// </summary>
        [DataMember]
        public int Score { get; set; }

        /// <summary>
        ///     The level achieved.
        /// </summary>
        [DataMember]
        public int Level { get; set; }

        /// <summary>
        ///     The users name.
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new entry of <see cref="UserScore" />.
        /// </summary>
        /// <param name="score">The score achieved.</param>
        /// <param name="level">The level achieved.</param>
        /// <param name="username">The users name.</param>
        public UserScore(int score, int level, string username)
        {
            this.Score = score;
            this.Level = level;
            this.Username = username;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns a string that represents the entry.
        /// </summary>
        /// <returns>A string that represents the entry.</returns>
        public override string ToString()
        {
            return this.Username + ", " + this.Score + ", " + this.Level;
        }

        #endregion
    }
}