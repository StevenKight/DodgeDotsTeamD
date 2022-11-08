using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     The Point Manager class
    /// </summary>
    public class PointManager : ICollection<PointObject>
    {
        #region Data members

        private const int MinRandomSpawnTick = 80;

        private readonly IList<PointObject> points;
        private readonly Canvas backgroundCanvas;
        private readonly GameSettings.PointType type;
        private Color color;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private int randomSpawnTick;
        private int maxRandomSpawnTick;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public int Count => this.points.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        public bool IsReadOnly => this.points.IsReadOnly;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PointManager" /> class.
        /// </summary>
        /// <param name="background">The background.</param>
        /// <param name="type">The type of point objects to spawn.</param>
        public PointManager(Canvas background, GameSettings.PointType type)
        {
            this.points = new List<PointObject>();
            this.backgroundCanvas = background;
            this.type = type;

            this.pointColor();

            this.randomSpawnTick = 0;
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.timer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<PointObject> GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.points).GetEnumerator();
        }

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        public void Add(PointObject item)
        {
            this.points.Add(item);
        }

        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            this.points.Clear();
        }

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        ///     true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(PointObject item)
        {
            return this.points.Contains(item);
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an
        ///     <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements
        ///     copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see>
        ///     must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(PointObject[] array, int arrayIndex)
        {
            this.points.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        ///     true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>;
        ///     otherwise, false. This method also returns false if item is not found in the original
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public bool Remove(PointObject item)
        {
            return this.points.Remove(item);
        }

        private void pointColor()
        {
            double maxDifference;

            switch (this.type)
            {
                case GameSettings.PointType.Basic:
                    this.color = GameSettings.BasicPointColor;

                    maxDifference = MinRandomSpawnTick * GameSettings.BasicRarity;
                    break;
                case GameSettings.PointType.Mid:
                    this.color = GameSettings.MidPointColor;

                    maxDifference = MinRandomSpawnTick * GameSettings.MidRarity;
                    break;
                case GameSettings.PointType.Max:
                    this.color = GameSettings.MaxPointColor;

                    maxDifference = MinRandomSpawnTick * GameSettings.MaxRarity;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.maxRandomSpawnTick = (int)(MinRandomSpawnTick + maxDifference);
        }

        /// <summary>
        ///     Removes all point objects from tacking and canvas.
        /// </summary>
        public void StopPointManager()
        {
            foreach (var point in this.points)
            {
                this.backgroundCanvas.Children.Remove(point.Sprite);
            }

            this.points.Clear();
            this.stopPointSpawn();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (this.tickCount == this.randomSpawnTick)
            {
                if (this.tickCount != 0)
                {
                    _ = this.createRandomPoint();
                }

                this.tickCount = 0;
                var randomSpawnNext = GameSettings.Random.Next(MinRandomSpawnTick, this.maxRandomSpawnTick);
                this.randomSpawnTick = randomSpawnNext;
            }
            else
            {
                this.tickCount++;
            }
        }

        private async Task createRandomPoint()
        {
            var point = new PointObject((int)this.type, this.color);

            var randomXPosition = GameSettings.Random.Next(0, (int)(this.backgroundCanvas.Width - point.Width) + 1);
            var randomYPosition = GameSettings.Random.Next(0, (int)(this.backgroundCanvas.Height - point.Height) + 1);

            point.X = randomYPosition;
            point.Y = randomXPosition;

            this.backgroundCanvas.Children.Add(point.Sprite);
            this.points.Add(point);

            var file = await GameSettings.AudioManager.AudioFolder.Result.GetFileAsync("PointAdded.wav");
            GameSettings.AudioManager.PlayAudio(file);
        }

        private void stopPointSpawn()
        {
            this.timer.Stop();
        }

        #endregion
    }
}