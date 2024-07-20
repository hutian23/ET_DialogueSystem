using System;
using System.Linq;

namespace AABB
{
    public class Box: IBox
    {
        #region Constructors

        public Box(World _world, float x, float y, float width, float height)
        {
            world = _world;
            bounds = new RectangleF(x, y, width, height);
        }

        #endregion

        #region Fields

        private readonly World world;
        private RectangleF bounds;

        #endregion

        #region Properties

        public float X => Bounds.X;
        public float Y => Bounds.Y;
        public float Width => Bounds.Width;
        public float Height => Bounds.Height;

        public RectangleF Bounds
        {
            get
            {
                return bounds;
            }
        }

        public object Data { get; set; }

        #endregion

        //Check Collision
        public IMovement Simulate(float x, float y, Func<ICollision, ICollisionResponse> filter)
        {
            return world.Simulate(this, x, y, filter);
        }

        public IMovement Simulate(float x, float y, Func<ICollision, CollisionResponses> filter)
        {
            return Move(x, y, (col) =>
            {
                if (col.Hit == null)
                {
                    return null;
                }

                return CollisionResponse.Create(col, filter(col));
            });
        }

        public IMovement Move(float x, float y, Func<ICollision, ICollisionResponse> filter)
        {
            var movement = Simulate(x, y, filter);
            bounds.X = movement.Destination.X;
            bounds.Y = movement.Destination.Y;
            world.Update(this, movement.Origin);
            return movement;
        }

        public IMovement Move(float x, float y, Func<ICollision, CollisionResponses> filter)
        {
            var movement = Simulate(x, y, filter);
            bounds.X = movement.Destination.X;
            bounds.Y = movement.Destination.Y;
            world.Update(this, movement.Origin);
            return movement;
        }

        private Enum tags;

        public IBox AddTags(params Enum[] newTags)
        {
            foreach (var tag in newTags)
            {
                AddTag(tag);
            }

            return this;
        }

        public IBox RemoveTags(params Enum[] newTags)
        {
            foreach (var tag in newTags)
            {
                RemoveTag(tag);
            }

            return this;
        }

        private void AddTag(Enum tag)
        {
            if (tags == null)
            {
                tags = tag;
            }
            else
            {
                var t = tags.GetType();
                var ut = Enum.GetUnderlyingType(t);
                if (ut != typeof (ulong))
                {
                    tags = (Enum)Enum.ToObject(t, Convert.ToInt64(tags) | Convert.ToInt64(tag));
                }
                else
                {
                    tags = (Enum)Enum.ToObject(t, Convert.ToUInt64(tags) | Convert.ToUInt64(tag));
                }
            }
        }

        private void RemoveTag(Enum tag)
        {
            if (tags != null)
            {
                var t = tags.GetType();
                var ut = Enum.GetUnderlyingType(t);

                if (ut != typeof (ulong))
                {
                    tags = (Enum)Enum.ToObject(t, Convert.ToInt64(tags) & ~Convert.ToInt64(tag));
                }
                else
                {
                    tags = (Enum)Enum.ToObject(t, Convert.ToUInt64(tags) & ~Convert.ToUInt64(tag));
                }
            }
        }

        public bool HasTag(params Enum[] values)
        {
            return (tags != null) && values.Any(value => tags.HasFlag(value));
        }

        public bool HasTags(params Enum[] values)
        {
            return (tags != null) && values.All(value => tags.HasFlag(value));
        }
    }
}