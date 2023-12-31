using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class SearchMenuWindowProvider: ScriptableObject, ISearchWindowProvider
    {
        private DialogueTreeView treeView;
        private DialogueEditor window;

        public void Init(DialogueEditor dialogueEditor, DialogueTreeView _treeView)
        {
            this.window = dialogueEditor;
            this.treeView = _treeView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("创建新节点")));
            
            //常用节点
            entries.Add(new SearchTreeGroupEntry(new GUIContent("常用")){level = 1});
            entries.Add(new SearchTreeEntry(new GUIContent("背景板")){level = 2, userData = new CommentBlockData()});
            
            this.LoadDialogueNode(entries);
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            // 鼠标在编辑器view内的位置
            var mousePosition = this.window.rootVisualElement.ChangeCoordinatesTo(this.window.rootVisualElement.parent,
                context.screenMousePosition - this.window.position.position);
            var graphPosition = this.treeView.contentViewContainer.WorldToLocal(mousePosition);

            switch (SearchTreeEntry.userData)
            {
                case Type type:
                {
                    this.treeView.CreateNode(type, graphPosition);
                    return true;
                }
                case CommentBlockData commentBlockData:
                {
                    this.treeView.CreateCommentBlock(graphPosition);
                    return true;
                }
            }

            return false;
        }

        private void LoadDialogueNode(List<SearchTreeEntry> entries)
        {
            Assembly assembly = typeof (DialogueNode).Assembly;
            //dialogueNode的子类
            List<Type> res = assembly.GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof (DialogueNode))).ToList();
            //节点目录树树状结构
            List<SearchWindowMenuItem> mainMenu = new();
            
            foreach (var type in res)
            {
                NodeTypeAttribute attr = type.GetCustomAttribute(typeof (NodeTypeAttribute)) as NodeTypeAttribute;
                if (attr == null || string.IsNullOrEmpty(attr.Level))
                {
                    continue;
                }

                string[] levels = attr.Level.Split('/');
                //遍历分割的每一项的名称
                List<SearchWindowMenuItem> currentFloor = mainMenu;
                for (int i = 0; i < levels.Length; i++)
                {
                    string currentName = levels[i];
                    bool exist = false; //是否存在这个目录
                    bool lastFloor = (i == levels.Length - 1); //不是最后一项，说明当前项还是菜单项
                    SearchWindowMenuItem temp = currentFloor.Find(item => item.Name == currentName);
                    //找到对应的层级了
                    if (temp != null)
                    {
                        exist = true;
                        currentFloor = temp.ChildItems;
                    }

                    //当前层级不存在，新建项添加到当前层级中
                    if (!exist)
                    {
                        SearchWindowMenuItem item = new() { Name = currentName, IsNode = lastFloor };
                        currentFloor.Add(item);
                        //如果当前项不是节点，且没有下一层
                        if (!item.IsNode && item.ChildItems == null)
                        {
                            //构建新的子层级
                            item.ChildItems = new List<SearchWindowMenuItem>();
                        }

                        if (item.IsNode) item.NodeType = type;
                        currentFloor = item.ChildItems;
                    }
                }
            }
            
            //递归创建目录树
            GenerateSearchTree(mainMenu, 1, ref entries);
        }

        public void GenerateSearchTree(List<SearchWindowMenuItem> floor, int floorIndex, ref List<SearchTreeEntry> treeEntries)
        {
            foreach (var item in floor)
            {
                //当前项不是节点
                if (!item.IsNode)
                {
                    SearchTreeEntry entry = new SearchTreeGroupEntry(new GUIContent(item.Name)) { level = floorIndex };
                    treeEntries.Add(entry);
                    //递归下一层
                    GenerateSearchTree(item.ChildItems, floorIndex + 1, ref treeEntries);
                }
                //当前是节点(到头了)
                else
                {
                    SearchTreeEntry entry = new(new GUIContent(item.Name)) { level = floorIndex, userData = item.NodeType };
                    treeEntries.Add(entry);
                }
            }
        }

        //存储节点目录的结构，参考了https://github.com/HalfADog/Unity-RPGCore-BehaviorTree/blob/main/Editor/BTSearchWindow.cs
        public class SearchWindowMenuItem
        {
            public string Name;
            public bool IsNode;
            public Type NodeType;
            public List<SearchWindowMenuItem> ChildItems;
        }
    }
}