using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public struct BBTestManagerCallback
    {
        public long instanceId;
        public int order;
        public int stop;
    }
    
    public struct ViewComponentReloadCallback
    {
        public long instanceId;
        public int ReloadType;
        public uint preView_TargetID;
        public string treeName; 
        public Language language;
    }

    public enum ViewRunMode
    {
        编辑器,
        运行时,
    }

    public static class ViewReloadType
    {
        public const int None = 0;
        public const int Reload = 1; //重新加载对话树，从头开始
        public const int Preview = 2; // 预览单个节点，需要执行rootNode的初始化
        public const int RuntimeReload = 3;
    }

    public class DialogueViewComponent: MonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        [LabelText("运行模式: ")]
        public ViewRunMode runMode;

        public bool EditorMode => runMode == ViewRunMode.编辑器;
        public bool RuntimeMode => runMode == ViewRunMode.运行时;

        [LabelText("语言: ")]
        public Language Language;

        [LabelText("引用: ")]
        public DialogueTree tree;

        [LabelText("克隆树: "), ShowIf("EditorMode")]
        public DialogueTree cloneTree;

        [HideInInspector]
        public List<SharedVariable> Variables = new();

        public DialogueNode GetNode(uint targetID)
        {
            if (cloneTree == null)
            {
                Debug.LogError("DialogueViewComponent cloneTree is null");
                return null;
            }

            if (cloneTree.targets.TryGetValue(targetID, out DialogueNode node))
            {
                DialogueNode cloneNode = MongoHelper.Clone(node);
                cloneNode.text = node.GetContent(Language);
                return cloneNode;
            }

            Debug.LogError($"cannot found node,targetID:{targetID}");
            return null;
        }

        public int GetLength()
        {
            return cloneTree.targets.Count;
        }
        
        [Button("导出对话树"), ShowIf("RuntimeMode")]
        public void ExportTree()
        {
            tree.Export();
        }

        [Button("对话树重载")]
        public void ReloadTree()
        {
            switch (runMode)
            {
                case ViewRunMode.编辑器:
                    cloneTree = tree.DeepClone();
                    if (Application.isPlaying)
                        EventSystem.Instance.Invoke(new ViewComponentReloadCallback()
                        {
                            instanceId = instanceId, ReloadType = ViewReloadType.Reload
                        });
                    break;
                case ViewRunMode.运行时:
                    if (Application.isPlaying)
                        EventSystem.Instance.Invoke(new ViewComponentReloadCallback()
                        {
                            instanceId = this.instanceId,
                            ReloadType = ViewReloadType.RuntimeReload,
                            treeName = tree.treeName,
                            language = Language
                        });
                    break;
            }
        }

        //TODO 打开视图的回调，因为Editor和loader是两个分开的程序集，现在只想到这个方法
        public Action OpenWindow = null;

        [Button("对话树视图"), ShowIf("EditorMode")]
        public void OpenTreeView()
        {
            this.OpenWindow?.Invoke();
        }

        [Button("保存对话树"), ShowIf("EditorMode")]
        public void SaveCloneTree()
        {
            if (tree == null)
            {
                Debug.LogError("引用为空!");
                return;
            }

            if (cloneTree == null)
            {
                Debug.LogError("克隆树为空!");
                return;
            }

            //有一些属性 eg. Status 不希望被保存 需要添加 BsonIgnore
            var cloneTree2 = MongoHelper.Clone(cloneTree);
            EditorUtility.CopySerialized(cloneTree2, tree);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}