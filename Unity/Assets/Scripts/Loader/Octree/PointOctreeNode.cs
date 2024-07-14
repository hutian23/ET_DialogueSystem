using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET.Client
{
    public class PointOctreeNode<T>
    {
        //center of this node
        public Vector3 Center { get; private set; }

        //Length of the sides of this node
        public float SideLength { get; private set; }

        //Minimum size for a node in this octree
        protected float minSize;

        //Boundings box that represents this node
        protected Bounds bounds = default;

        //Obbject in this node <--- GameObject?
        protected readonly List<OctreeObject> objects = new();

        //Child nodes, if any
        protected PointOctreeNode<T>[] children = null;

        protected bool HasChildren
        {
            get
            {
                return children != null;
            }
        }

        //bounds of potential children to this node. these are actual size(with looseness taken into account),not base size
        protected Bounds[] childBounds;

        //if there are already NUM_OBJECTS_ALLOWED(当前节点数量限制了) in a node,we split it info childre
        protected const int NUM_OBJECTS_ALLOWED = 8;

        //For reverting the bounds size after temporary changes
        protected Vector3 actualBoundsSize;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseLengthVal">Length of this node. not taking looseness into account.</param>
        /// <param name="minSizeVal">Minimum size of nodes in this octree.</param>
        /// <param name="centerVal">Center position of this node.</param>
        public PointOctreeNode(float baseLengthVal, float minSizeVal, Vector3 centerVal)
        {
            SetValues(baseLengthVal, minSizeVal, centerVal);
        }

        /// <summary>
        /// Add an object
        /// </summary>
        /// <param name="obj">Object to add</param>
        /// <param name="objPos">Position of the object</param>
        /// <returns></returns>
        public bool Add(T obj, Vector3 objPos)
        {
            if (!Encapsulates(bounds, objPos))
            {
                return false;
            }

            SubAdd(obj, objPos);
            return true;
        }

        /// <summary>
        /// Remove an object. Makes the assumption that the obj only exists once in the tree
        /// </summary>
        /// <param name="obj">Object to remove</param>
        /// <returns>True if the obj was removed successfully</returns>
        public bool Remove(T obj)
        {
            bool removed = false;

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Obj.Equals(obj))
                {
                    removed = objects.Remove(objects[i]);
                    break;
                }
            }

            if (!removed && children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    removed = children[i].Remove(obj);
                    if (removed)
                    {
                        break;
                    }
                }
            }

            if (removed && children != null)
            {
                //Check if we should merge nodes now that we've removed an item
                if (ShouldMerge())
                {
                    Merge();
                }
            }

            return removed;
        }

        /// <summary>
        /// Removes the specified object at the given position.Makes the assumption that the obj
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objPos"></param>
        /// <returns></returns>
        public bool Remove(T obj, Vector3 objPos)
        {
            if (!Encapsulates(bounds, objPos))
            {
                return false;
            }

            return SubRemove(obj, objPos);
        }

        /// <summary>
        /// Remove objects that are within maxDistance of the specified ray
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="maxDistance">Maximum distanec from the ray to consider.</param>
        /// <param name="result">List result.</param>
        public void GetNearby(ref Ray ray, float maxDistance, List<T> result)
        {
            //Does the ray hit this node at all?
            //Notes: Expanding the bounds is not exactly the same as a real distance check.but it's fast.
            //TODO: Does someone have a fast And accurate formula to do this check?
            bounds.Expand(new Vector3(maxDistance * 2, maxDistance * 2, maxDistance * 2)); //松散
            //ray是否和bounds重叠?
            bool intersected = bounds.IntersectRay(ray);
            bounds.size = actualBoundsSize;
            if (!intersected)
            {
                return;
            }

            //Check against any objects in this node
            for (int i = 0; i < objects.Count; i++)
            {
                if (SqrtDistanceToRay(ray, objects[i].Pos) <= (maxDistance * maxDistance))
                {
                    result.Add(objects[i].Obj);
                }
            }

            //Check children
            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    children[i].GetNearby(ref ray, maxDistance, result);
                }
            }
        }

        /// <summary>
        /// Return objects that are within the specified position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <param name="result"></param>
        public void GetNearby(ref Vector3 position, float maxDistance, List<T> result)
        {
            float sqrtMaxDistance = maxDistance * maxDistance;

#if UNITY_2017_1_OR_NEWER
            //Does the node intersect with the sphere of center = position and radius = maxDistance?
            if ((bounds.ClosestPoint(position) - position).sqrMagnitude > sqrtMaxDistance)
            {
                return;
            }
#else
            //Does the ray hit this node at all
            //Notes: Expanding the bounds is not exactly the same as a real distance check,but it's fast
            //TODO fast an accurate check
            bounds.Expand(new Vector3(maxDistance * 2, maxDistance * 2, maxDistance * 2));
            bool contained = bounds.Contains(position);
            bounds.size = actualBoundsSize;
            if (!contained)
            {
                return;
            }
#endif

            //Check against any object in this node
            for (int i = 0; i < objects.Count; i++)
            {
                if ((position - objects[i].Pos).sqrMagnitude <= sqrtMaxDistance)
                {
                    result.Add(objects[i].Obj);
                }
            }

            //Check children
            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    children[i].GetNearby(ref position, maxDistance, result);
                }
            }
        }

        /// <summary>
        /// Returns all objects in the tree
        /// </summary>
        /// <param name="result"></param>
        public void GetAll(List<T> result)
        {
            //add directly contained objects
            result.AddRange(objects.Select(o => o.Obj));

            //add children objects
            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    children[i].GetAll(result);
                }
            }
        }

        /// <summary>
        /// Set the 8 children of the octree
        /// </summary>
        /// <param name="childOctrees"></param>
        public void SetChildren(PointOctreeNode<T>[] childOctrees)
        {
            if (childOctrees.Length != 8)
            {
                Debug.LogError("Child octree array must be length 8. was length: " + childOctrees.Length);
                return;
            }

            children = childOctrees;
        }

        /// <summary>
        /// Draws node boundaries visually for debugging
        /// Musted be called from onDrawGizmos externally.see also: DrawAllObjects.
        /// </summary>
        /// <param name="depth"></param>
        public void DrawAllBounds(float depth = 0)
        {
            float tinVal = depth / 7; // Will eventually get values > 1. Color rounds to 1 automatically
            Gizmos.color = new Color(tinVal, 0, 1.0f - tinVal);

            Bounds thisBounds = new(Center, new Vector3(SideLength, SideLength, SideLength));
            Gizmos.DrawWireCube(thisBounds.center, thisBounds.size);

            if (children != null)
            {
                depth++;
                for (int i = 0; i < 8; i++)
                {
                    children[i].DrawAllBounds(depth);
                }
            }

            Gizmos.color = Color.white;
        }

        /// <summary>
        /// Draws the bounds of all objects in the tree visually for debugging
        /// Must be called from onDrawGizmos externally. See also: DrawAllBounds
        ///TODO NOTE: marker.tif must be placed in your Unity /Assets/Gizmos subfolder for this to work.
        /// </summary>
        public void DrawAllObjects()
        {
            float tintVal = SideLength / 20;
            Gizmos.color = new Color(0, 1.0f - tintVal, 0.25f);

            foreach (OctreeObject obj in objects)
            {
                Gizmos.DrawIcon(obj.Pos, "marker.tif", true);
            }

            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    children[i].DrawAllObjects();
                }
            }

            Gizmos.color = Color.white;
        }

        /// <summary>
        /// We can shrink(收缩) the octree if:
        /// - This node is >= double minLength in length
        /// - All Objects in the root node are within one octant(象限?)
        /// - this node doesn't have children, or does but 7/8 children are emptu
        /// we can also shrink it if there are no objects left at all!  
        /// </summary>
        /// <param name="minLength"></param>
        /// <returns></returns>
        public PointOctreeNode<T> ShrinkIfPossible(float minLength)
        {
            if (SideLength < (2 * minLength))
            {
                return this;
            }

            if (objects.Count == 0 && (children == null || children.Length == 0))
            {
                return this;
            }

            //TODO Check object in root
            int bestFit = -1;
            for (int i = 0; i < objects.Count; i++)
            {
                OctreeObject curObj = objects[i];
                int newBestFit = BestFitChild(curObj.Pos);
                if (i == 0 || newBestFit == bestFit)
                {
                    bestFit = newBestFit;
                }
                else
                {
                    return this; // Can't reduce - objects fit in different octants
                }
            }

            //Checks objects in children if there are any
            if (children != null)
            {
                bool childHadContent = false;
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].HasAnyObjects())
                    {
                        if (childHadContent)
                        {
                            return this; // Can't shrink - another child had content already
                        }

                        if (bestFit >= 0 && bestFit != i)
                        {
                            return this; // Can't reduce - objects in root are in a different octant in child
                        }

                        childHadContent = true;
                        bestFit = i;
                    }
                }
            }

            //Can reduce
            if (children == null)
            {
                //We don't have any children,so just shrink this node to the new size
                //We already know that everything will still fit in it
                SetValues(SideLength / 2, minSize, childBounds[bestFit].center);
                return this;
            }

            //We have children. Use the appropriate child as the new root node
            return children[bestFit];
        }

        /// <summary>
        /// Find which child node this object would be most likely to fit in.
        /// </summary>
        /// <param name="objBoundsCenter">The object's bounds/</param>
        /// <returns>One of the eight child octants. </returns>
        public int BestFitChild(Vector3 objBoundsCenter)
        {
            return (objBoundsCenter.x <= Center.x? 0 : 1) + (objBoundsCenter.y >= Center.y? 0 : 4) + (objBoundsCenter.z >= Center.z? 0 : 2);
        }

        /// <summary>
        /// Checks if this node or anything bellow it has something in it
        /// </summary>
        /// <returns></returns>
        public bool HasAnyObjects()
        {
            if (objects.Count > 0)
            {
                return true;
            }

            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (children[i].HasAnyObjects())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Set values for this node
        /// </summary>
        /// <param name="baseLengthVal">Length of this node, not taking looseness into account.</param>
        /// <param name="minSizeVal">Minimum size of nodes in this octree.</param>
        /// <param name="centerVal">Center position of this node.</param>
        protected void SetValues(float baseLengthVal, float minSizeVal, Vector3 centerVal)
        {
            SideLength = baseLengthVal;
            minSize = minSizeVal;
            Center = centerVal;

            //Create the bounding box
            actualBoundsSize = new Vector3(SideLength, SideLength, SideLength);
            bounds = new Bounds(Center, actualBoundsSize);

            float quarter = SideLength / 4f;
            float childActualLength = SideLength / 2;
            Vector3 childActualSize = new Vector3(childActualLength, childActualLength, childActualLength);
            childBounds = new Bounds[8];
            childBounds[0] = new Bounds(Center + new Vector3(-quarter, quarter, -quarter), childActualSize);
            childBounds[1] = new Bounds(Center + new Vector3(quarter, quarter, -quarter), childActualSize);
            childBounds[2] = new Bounds(Center + new Vector3(-quarter, quarter, quarter), childActualSize);
            childBounds[3] = new Bounds(Center + new Vector3(quarter, quarter, quarter), childActualSize);
            childBounds[4] = new Bounds(Center + new Vector3(-quarter, -quarter, -quarter), childActualSize);
            childBounds[5] = new Bounds(Center + new Vector3(quarter, -quarter, -quarter), childActualSize);
            childBounds[6] = new Bounds(Center + new Vector3(-quarter, -quarter, quarter), childActualSize);
            childBounds[7] = new Bounds(Center + new Vector3(quarter, -quarter, quarter), childActualSize);
        }

        /// <summary>
        /// Private counterpart to the public add method
        /// </summary>
        /// <param name="obj">Object to add</param>
        /// <param name="objPos">Position of the object</param>
        protected void SubAdd(T obj, Vector3 objPos)
        {
            //we know it fits at this level if we 've got this far

            //we always put things in the deepest possible child
            //So we can skip checks and simply move down if there are children already
            if (!HasChildren)
            {
                //Just add if few objecs are here, or children would be below min size
                if (objects.Count < NUM_OBJECTS_ALLOWED || (SideLength / 2) < minSize)
                {
                    OctreeObject newObj = new() { Obj = obj, Pos = objPos };
                    objects.Add(newObj);
                    return; // We're done. no children yet
                }

                //Enough objects in ths node already: create the 8 children
                int bestFitChild;
                if (children == null)
                {
                    Split();
                    if (children == null)
                    {
                        Debug.LogError("Child creation failed for an unknown reason. Early exit");
                        return;
                    }

                    //Now that we have the new children,move this node's existing objects into them
                    for (int i = objects.Count - 1; i >= 0; i--)
                    {
                        OctreeObject existingObj = objects[i];
                        //Find which child the objects is closest to based on where the object's center is located in relation to the octree's center
                        bestFitChild = BestFitChild(existingObj.Pos);
                        children[bestFitChild].SubAdd(existingObj.Obj, existingObj.Pos); // Go a level deeper
                        objects.Remove(existingObj);
                    }
                }
            }

            //Handle the new object we're adding now
            int bestFit = BestFitChild(objPos);
            children[bestFit].SubAdd(obj, objPos);
        }

        /// <summary>
        /// Private counterpart to the public <see cref = "Remove(T,Vector3)"/> method.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objPos"></param>
        /// <returns></returns>
        protected bool SubRemove(T obj, Vector3 objPos)
        {
            bool removed = false;

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Obj.Equals(obj))
                {
                    removed = objects.Remove(objects[i]);
                    break;
                }
            }

            if (!removed && children != null)
            {
                int bestFitChild = BestFitChild(objPos);
                removed = children[bestFitChild].SubRemove(obj, objPos);
            }

            if (removed && children != null)
            {
                //Check if we should merge nodes now that we've removed an item
                if (ShouldMerge())
                {
                    Merge();
                }
            }

            return removed;
        }

        /// <summary>
        /// Splits the octree into eight children
        /// </summary>
        protected void Split()
        {
            float quarter = SideLength / 4f;
            float newLength = SideLength / 2;
            children = new PointOctreeNode<T>[8];
            children[0] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(-quarter, quarter, -quarter));
            children[1] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(quarter, quarter, -quarter));
            children[2] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(-quarter, quarter, quarter));
            children[3] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(quarter, quarter, quarter));
            children[4] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(-quarter, -quarter, -quarter));
            children[5] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(quarter, -quarter, -quarter));
            children[6] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(-quarter, -quarter, quarter));
            children[7] = new PointOctreeNode<T>(newLength, minSize, Center + new Vector3(quarter, -quarter, quarter));
        }

        /// <summary>
        /// Merge all children into this node - the opposite of split
        /// Note: we only have to check one level down since a'merge will never happen if the children already have children
        /// 
        /// </summary>
        protected void Merge()
        {
            //Note: We know children != null or we wouldn't merging
            for (int i = 0; i < 8; i++)
            {
                PointOctreeNode<T> curChild = children[i];
                int numObjects = curChild.objects.Count;
                for (int j = numObjects - 1; j >= 0; j--)
                {
                    OctreeObject curObj = curChild.objects[j];
                    objects.Add(curObj);
                }
            }

            //Remove the child nodes (and the objects in them - they've been added elsewhere now)
            children = null;
        }

        /// <summary>
        /// Checks if outerBounds encapsulates the given point
        /// </summary>
        /// <param name="outerBounds">Outer bounds</param>
        /// <param name="point">Point</param>
        /// <returns>True if innerBounds is fully encapsulated by outerBounds</returns>
        protected static bool Encapsulates(Bounds outerBounds, Vector3 point)
        {
            return outerBounds.Contains(point);
        }

        /// <summary>
        /// Checks if there are few enough objects in this node and its children that the children should all be merged into this.
        /// </summary>
        /// <returns>True there are less or the same about the objects in this and its children than numObjectsAllowed. </returns>
        protected bool ShouldMerge()
        {
            int totalObjects = objects.Count;
            if (children != null)
            {
                foreach (PointOctreeNode<T> child in children)
                {
                    if (child.children != null)
                    {
                        //If any of the children have children. there are definitely too many to merge
                        //or the child would have been merged already
                        return false;
                    }

                    totalObjects += child.objects.Count;
                }
            }

            return totalObjects <= NUM_OBJECTS_ALLOWED;
        }

        /// <summary>
        /// Checks if outerBounds encapsulates the given point
        /// </summary>
        /// <returns>Squared distance from the point to the closest point of the ray</returns>
        public static float SqrtDistanceToRay(Ray ray, Vector3 point)
        {
            return Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude;
        }

        //An Object in the octree
        [Serializable]
        public class OctreeObject
        {
            public T Obj;

            public Vector3 Pos;
        }
    }
}