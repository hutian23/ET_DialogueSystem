using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    //A dynamic octree for storing any objects that can be described as a single point
    //See also: BoundsOctree,where objects are described by AABB bounds
    // OcTree:
    //- Any Octree is a tree data structure whic divides 3D space into smaller partitions(nodes)
    //- any places objects into the appropriate nodes. This allows fast access to objects in an area of interest without have to check
    //- every object.
    //Dynamic: 
    //- The octree grows or shrink as required when objects as added or removed
    //- It also splits and merges nodes as appropriate. There is no maximum depth.
    //- nodes have a constant - numObjectsAllowed - which sets the amount of items allowed in a node before it splits.
    public class PointOctree<T>
    {
        //The total amount of objects currently in the tree
        public int Count { get; private set; }

        //Root node of the octree
        protected PointOctreeNode<T> rootNode;
        
        //Size that the octree was on creation
        protected readonly float initialSize;

        //Minimum side length that a node can be -essentially an alternative to having a max depth
        protected readonly float minSize;

        /// <summary>
        /// Constructor for the point octree.
        /// </summary>
        /// <param name="initialWorldSize">Size of the sides of the initial node. The octree will never shrink smaller than this.</param>
        /// <param name="initialWorldPos"></param>
        /// <param name="minNodeSize"></param>
        public PointOctree(float initialWorldSize, Vector3 initialWorldPos, float minNodeSize)
        {
            if (minNodeSize > initialWorldSize)
            {
                Debug.LogWarning("Minimum node size must be at least as big as the initial world size. Was: " + minNodeSize + " Adjusted to: " + initialWorldSize);
                minNodeSize = initialWorldSize;
            }

            Count = 0;
            initialSize = initialWorldSize;
            minSize = minNodeSize;
            rootNode = new PointOctreeNode<T>(initialWorldSize, minSize, initialWorldPos);
        }

        /// <summary>
        /// Add an object
        /// </summary>
        public void Add(T obj, Vector3 objPos)
        {
            //Add object or expand the octree untill it can be added
            int count = 0; // Safety check against infinite / excessive growth
            while (!rootNode.Add(obj,objPos))
            {
            }
        }

        public bool Remove(T obj)
        {
            return false;
        }

        public bool Remove(T obj, Vector3 objPos)
        {
            return false;
        }

        public bool GetNearbyNonAlloc(Ray ray, float maxDistance, List<T> nearby)
        {
            return false;
        }

        public T[] GetNearby(Ray ray, float maxDistance)
        {
            return null;
        }

        public T[] GetNearby(Vector3 position, float maxDistance)
        {
            return null;
        }

        public bool GetNearbyNonAlloc(Vector3 position, float maxDistance, List<T> nearBy)
        {
            return false;
        }

        /// <summary>
        /// Return all objects in the tree
        /// If none, returns an empty array(not null)
        /// </summary>
        /// <returns></returns>
        public ICollection<T> GetAll()
        {
            List<T> objects = new List<T>(Count);
            rootNode.GetAll(objects);
            return objects;
        }

        /// <summary>
        /// Draws node boundaries visually for debugging
        /// </summary>
        public void DrawAllBounds()
        {
            rootNode.DrawAllBounds();
        }

        /// <summary>
        /// Draws the bounds of all objects in the tree visually for debugging
        /// Must be called from OnDrawGizmos externally. See also: DrawAllBounds.
        /// </summary>
        public void DraeAllObjects()
        {
            rootNode.DrawAllObjects();
        }

        protected void Grow(Vector3 direction)
        {
            int xDirection = direction.x >= 0? 1 : -1;
            int yDirection = direction.y >= 0? 1 : -1;
            int zDirection = direction.z >= 0? 1 : -1;
            PointOctreeNode<T> oldRoot = rootNode;
        }
    }
}