namespace AABB
{
    //handle collision event
    public class Collision: ICollision
    {
        public IBox Box { get; set; }

        public IBox Other
        {
            get
            {
                return Hit?.Box;
            }
        }

        public RectangleF Origin { get; set; }
        public RectangleF Goal { get; set; }
        public IHit Hit { get; set; }
        public bool HasCollided => Hit != null;

        public Collision()
        {
        }
    }
}