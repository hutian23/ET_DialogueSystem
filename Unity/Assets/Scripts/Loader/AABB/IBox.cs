using System;
using System.Drawing;

namespace AABB
{
    /// <summary>
    /// Represent a physical body in the world
    /// </summary>
    public interface IBox
    {
        #region Properties

        //Top left corner X coordinate of the box
        public float X { get; }

        //the top left corner Y coordinate of the box
        public float Y { get; }

        public float Width { get; }

        public float Height { get; }

        public RectangleF Bounds { get; }

        //custom user data attached to this box
        public object Data { get; set; }

        #endregion

        #region Movements

        /// <summary>
        /// Tries to move the box to specified coordinates with collision detection
        /// </summary>
        public IMovement Move(float x, float y, Func<ICollision, ICollisionResponse> filter);

        public IMovement Move(float x, float y, Func<ICollision, CollisionResponses> filter);

        /// <summary>
        /// Simulate the move of the box to specified coordinates with collision detection. the box's position isn;t altered.
        /// </summary>
        public IMovement Simulate(float x, float y, Func<ICollision, ICollisionResponse> filter);

        #endregion

        #region Tags

        /// <summary>
        /// Adds the tags to the box.
        /// </summary>
        public IBox AddTags(params Enum[] newTags);

        /// <summary>
        /// Remove the tags from the box.
        /// </summary>
        public IBox RemoveTags(params Enum[] newTags);

        /// <summary>
        /// Indicates whether the box has at least one of the given tags
        /// </summary>
        public bool HasTag(params Enum[] values);

        /// <summary>
        /// Indicated whether the box has all of the given tags.
        /// </summary>
        public bool HasTags(params Enum[] values);

        #endregion
    }
}