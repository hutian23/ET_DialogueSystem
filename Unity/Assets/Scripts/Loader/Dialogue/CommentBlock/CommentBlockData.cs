using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [Serializable]
    public class CommentBlockData
    {
        public Vector2 position;
        public string title = "Comment Block";
        public HashSet<string> children = new();
    }
}