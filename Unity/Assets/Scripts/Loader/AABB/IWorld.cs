using System;
using System.Collections.Generic;

namespace AABB
{
    /// <summary>
    /// Represents a physical world that contains AABB box colliding bodies
    /// </summary>
    public interface IWorld
    {
        #region Boxes

        /// <summary>
        /// Create a new box in the world
        /// </summary>
        public IBox Create(float x, float y, float width, float height);

        /// <summary>
        /// Remove the specified box from the world
        /// </summary>
        public bool Remove(IBox box);

        /// <summary>
        /// Update the specified box in the world(needed to be called to update spacial hash
        /// </summary>
        public void Update(IBox box, RectangleF from);

        #endregion

        #region Queries

        //Find the boxes contained in the given area of the world.
        public IEnumerable<IBox> Find(float x, float y, float w, float h);

        //Find the boxes contained in the given area of the world.
        public IEnumerable<IBox> Find(RectangleF area);

        #endregion

        #region Hits

        //Queires the world to find the nearest colliding point from a given position.
        public IHit Hit(Vector2 point, IEnumerable<IBox> ignoring = null);

        //Queries 
        public IHit Hit(RectangleF origin, RectangleF destination, IEnumerable<IBox> ignoring = null);

        #endregion

        #region Movements

        /// <summary>
        /// Simulate the specified box movement without moving it.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IMovement Simulate(Box box, float x, float y, Func<ICollision, ICollisionResponse> filter);

        #endregion

        #region Diagnostics

        /// <summary>
        /// Draw the debug layer
        /// </summary>
        public void DrawDebug(int x, int y, int w, int h, Action<int, int, int, int, float> drawCell, Action<IBox> drawBox,
        Action<string, int, int, float> drawString);

        #endregion
    }
}