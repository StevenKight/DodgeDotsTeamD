using System.Collections.ObjectModel;
using Windows.UI;
using static DodgeDots.Model.GameSettings;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Stores the game settings for level one.
    /// </summary>
    public class LevelOne : Level
    {
        #region Constructors

        /// <summary>
        ///     Initiates the <see cref="LevelOne" /> settings.
        /// </summary>
        public LevelOne()
        {
            Title = "Level 1";
            GameSurvivalTime = 25; // 25
            WaveInterval = 5;

            Waves = new Collection<Wave>
            {
                Wave.North,
                Wave.West,
                Wave.South,
                Wave.East
            };

            WaveColors = new Collection<Color>
            {
                Colors.MediumSpringGreen,
                Colors.DarkViolet
            };

            PointTypes = new Collection<PointType>
            {
                PointType.Basic
            };
        }

        #endregion
    }

    /// <summary>
    ///     Stores the game settings for level two.
    /// </summary>
    public class LevelTwo : Level
    {
        #region Constructors

        /// <summary>
        ///     Initiates the <see cref="LevelTwo" /> settings.
        /// </summary>
        public LevelTwo()
        {
            Title = "Level 2";
            GameSurvivalTime = 30; // 30
            WaveInterval = 5;

            Waves = new Collection<Wave>
            {
                Wave.North,
                Wave.West,
                Wave.South,
                Wave.East,
                Wave.NsFinalBlitz
            };

            WaveColors = new Collection<Color>
            {
                Colors.MediumSpringGreen,
                Colors.DarkViolet,
                Colors.Chocolate,
                Colors.Maroon
            };

            FinalColor = Colors.Yellow;

            PointTypes = new Collection<PointType>
            {
                PointType.Basic,
                PointType.Mid
            };
        }

        #endregion
    }

    /// <summary>
    ///     Stores the game settings for level Three.
    /// </summary>
    public class LevelThree : Level
    {
        #region Constructors

        /// <summary>
        ///     Initiates the <see cref="LevelThree" /> settings.
        /// </summary>
        public LevelThree()
        {
            Title = "Level 3";
            GameSurvivalTime = 35; // 35
            WaveInterval = 5;

            Waves = new Collection<Wave>
            {
                Wave.North,
                Wave.West,
                Wave.South,
                Wave.East,
                Wave.DiagonalFinalBlitz
            };

            WaveColors = new Collection<Color>
            {
                Colors.MediumSpringGreen,
                Colors.DarkViolet,
                Colors.Chocolate,
                Colors.Maroon
            };

            FinalColor = Colors.Yellow;

            PointTypes = new Collection<PointType>
            {
                PointType.Basic,
                PointType.Mid,
                PointType.Max
            };
        }

        #endregion
    }

    /// <summary>
    ///     Base level class to define what information to store.
    /// </summary>
    public abstract class Level
    {
        #region Properties

        /// <summary>
        ///     The title of the level.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The length of the level in seconds.
        /// </summary>
        public int GameSurvivalTime { get; set; }

        /// <summary>
        ///     The interval between waves in the level in seconds.
        /// </summary>
        public int WaveInterval { get; set; }

        /// <summary>
        ///     List of waves in the level.
        /// </summary>
        public Collection<Wave> Waves { get; set; }

        /// <summary>
        ///     A list of wave colors for all except final blitz.
        /// </summary>
        public Collection<Color> WaveColors { get; set; }

        /// <summary>
        ///     The color of the final blitz wave if one is present.
        /// </summary>
        public Color FinalColor { get; set; }

        /// <summary>
        ///     The point objects that spawn in the level.
        /// </summary>
        public Collection<PointType> PointTypes { get; set; }

        #endregion
    }
}