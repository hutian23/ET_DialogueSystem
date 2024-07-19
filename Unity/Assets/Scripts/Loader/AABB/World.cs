using System;
using System.Collections.Generic;
using System.Linq;

namespace AABB
{
    public class World
    {
        public RectangleF Bounds;

        public World(float width, float height, float cellSize)
        {
            int iwidth = (int)Math.Ceiling(width / cellSize);
            int iheight = (int)Math.Ceiling(height / cellSize);

            grid = new Grid(iwidth, iheight, cellSize);
        }

        #region Boxes

        private Grid grid;

        public IBox Create(float x, float y, float width, float height)
        {
            Box box = new(this, x, y, width, height);
            grid.Add(box);
            return null;
        }

        public IEnumerable<IBox> Find(float x, float y, float w, float h)
        {
            x = Math.Max(0, Math.Min(x, Bounds.Right - w));
            y = Math.Max(0, Math.Min(y, Bounds.Bottom - h));

            return grid.QueryBoxes(x, y, w, h);
        }

        public IEnumerable<IBox> Find(RectangleF area)
        {
            return Find(area.X, area.Y, area.Width, area.Height);
        }

        public bool Remove(IBox box)
        {
            return grid.Remove(box);
        }

        public void Update(IBox box, RectangleF from)
        {
            grid.Update(box, from);
        }

        #endregion

        #region Hits

        public IHit Hit(Vector2 point, IEnumerable<IBox> ignoring = null)
        {
            var boxes = Find(point.X, point.Y, 0, 0);
            if (ignoring != null)
            {
                boxes = boxes.Except(ignoring);
            }

            foreach (IBox other in boxes)
            {
                IHit hit = AABB.Hit.Resolve(point, other);
                if (hit != null)
                {
                    return hit;
                }
            }

            return null;
        }

        public IHit Hit(Vector2 origin, Vector2 destination, IEnumerable<IBox> ignoring = null)
        {
            Vector2 min = Vector2.Min(origin, destination);
            Vector2 max = Vector2.Max(origin, destination);

            RectangleF wrap = new RectangleF(min, max - min);
            var boxes = Find(wrap.X, wrap.Y, wrap.Width, wrap.Height);

            if (ignoring != null)
            {
                boxes = boxes.Except(ignoring);
            }

            IHit nearest = null;

            foreach (var other in boxes)
            {
                var hit = AABB.Hit.Resolve(origin, destination, other);

                if (hit != null && (nearest == null) || hit.IsNearest(nearest, origin))
                {
                    nearest = hit;
                }
            }

            return nearest;
        }

        public IHit Hit(RectangleF origin, RectangleF destination, IEnumerable<IBox> ignoring = null)
        {
            var wrap = new RectangleF(origin, destination);
            var boxes = Find(wrap.X, wrap.Y, wrap.Width, wrap.Height);

            if (ignoring != null)
            {
                boxes = boxes.Except(ignoring);
            }

            IHit nearest = null;

            foreach (var other in boxes)
            {
                var hit = AABB.Hit.Resolve(origin, destination, other);

                if (hit != null && (nearest == null || hit.IsNearest(nearest, origin.Location)))
                {
                    nearest = hit;
                }
            }

            return nearest;
        }

        #endregion

        #region Movements

        public IMovement Simulate(Box box, float x, float y, Func<ICollision, ICollisionResponse> filter)
        {
            var origin = box.Bounds;
            var destination = new RectangleF(x, y, box.Width, box.Height);

            var hits = new List<IHit>();

            var result = new Movement()
            {
                Origin = origin,
                Goal = destination,
                Destination = Simulate(hits, new List<IBox>() { box }, box, origin, destination, filter),
                Hits = hits
            };
            return result;
        }

        public RectangleF Simulate(List<IHit> hits, List<IBox> ignoring, Box box, RectangleF origin, RectangleF destination,
        Func<ICollision, ICollisionResponse> filter)
        {
            var nearest = Hit(origin, destination, ignoring);

            if (nearest != null)
            {
                hits.Add(nearest);

                var impact = new RectangleF(nearest.Position, origin.Size);
                var collision = new Collision() { Box = box, Hit = nearest, Goal = destination };
                var response = filter(collision);

                if (response != null && destination != response.Destination)
                {
                    ignoring.Add(nearest.Box);
                    return Simulate(hits, ignoring, box, impact, response.Destination, filter);
                }
            }

            return destination;
        }

        #endregion

        #region Diagnostics

        public void DrawDebug(int x, int y, int w, int h, Action<int, int, int, int, float> drawCell, Action<IBox> drawBox,
        Action<string, int, int, float> drawString)
        {
            //Drawing boxes
            var boxes = grid.QueryBoxes(x, y, w, h);
            foreach (var box in boxes)
            {
                drawBox(box);
            }

            //Drawing cells
            var cells = grid.QueryCells(x, y, w, h);
            foreach (var cell in cells)
            {
                var count = cell.Count();
                var alpha = count > 0? 1f : 0.4f;
                drawCell((int)cell.Bounds.X, (int)cell.Bounds.Y, (int)cell.Bounds.Width, (int)cell.Bounds.Height, alpha);
                drawString(count.ToString(), (int)cell.Bounds.Center.X, (int)Bounds.Center.Y, alpha);
            }
        }

        #endregion
    }
}