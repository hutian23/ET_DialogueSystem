namespace AABB
{
    //handle collision event
    public class Collision: ICollision
    {
        public IBox Box { get; }

        public IBox Other
        {
            get
            {
                return Hit?.Box;
            }
        }

        public RectangleF Goal { get; }
        public IHit Hit { get; }

        public Collision()
        {
        }
    }
}