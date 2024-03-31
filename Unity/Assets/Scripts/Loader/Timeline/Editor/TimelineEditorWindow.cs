using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineEditorWindow: EditorWindow, ISelection
    {
        protected VisualElement m_Top;
        protected VisualElement m_LeftPanel;
        protected VisualElement m_TrackHierachy;
        protected VisualElement m_Toolbar;
        protected VisualElement m_TrackHandleContainer;

        protected ObjectField m_TargetField;
        protected Button m_PlayButton;
        protected Button m_PauseButton;
        protected FloatField m_PlaySpeedField;

        protected TimelineFieldView m_TimelineField;
        public Timeline Timeline { get; private set; }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            m_Top = root.Q("top");
            m_Top.SetEnabled(false);

            m_TargetField = root.Q<ObjectField>("target-field");
            m_TargetField.objectType = typeof (TimelinePlayer);
            m_TargetField.allowSceneObjects = true;
            m_TargetField.RegisterValueChangedCallback(e =>
            {
                //对象是否为持久化对象 
                //what is 持久化对象? 在 Scene 保存的 gameObject
                if (!EditorUtility.IsPersistent(e.newValue) && e.newValue is TimelinePlayer timelinePlayer &&
                    Timeline.TimelinePlayer != timelinePlayer)
                {
                    if (Timeline.TimelinePlayer)
                    {
                        Timeline.TimelinePlayer.Dispose();
                    }

                    if (!timelinePlayer.IsValid)
                    {
                        timelinePlayer.Init();
                        timelinePlayer.AddTimeline(Timeline);
                    }
                    else if (e.newValue == null)
                    {
                        if (Timeline.TimelinePlayer)
                        {
                            Timeline.TimelinePlayer.Dispose();
                        }
                    }
                    else
                    {
                        m_TargetField.SetValueWithoutNotify(null);
                    }
                }
            });
            m_TargetField.SetEnabled(!Application.isPlaying);
            m_PlayButton = root.Q<Button>("play-button");
            m_PlayButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = true; };

            m_PauseButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = false; };

            m_PlaySpeedField = root.Q<FloatField>();
            m_PlaySpeedField.RegisterValueChangedCallback((e) => { Timeline.TimelinePlayer.PlaySpeed = e.newValue; });

            m_LeftPanel = root.Q("left-panel");
            m_LeftPanel.SetEnabled(false);

            m_TrackHierachy = root.Q("track-hierachy");
            m_Toolbar = root.Q("tool-bar");
            m_TrackHandleContainer.focusable = true;
            m_TrackHandleContainer.RegisterCallback<KeyDownEvent>((e) =>
            {
                switch (e.keyCode)
                {
                    case KeyCode.Delete:
                    {
                        Timeline.ApplyModify(() =>
                        {
                            var selectableToRemove = Selections.ToList();
                            foreach (var selectable in selectableToRemove)
                            {
                                
                            }
                        }, "Remove");
                        break;
                    }
                }
            });
        }

        public VisualElement ContentContainer { get; }
        public List<ISelectable> Elements { get; }
        public List<ISelectable> Selections { get; }

        public void AddToSelection(ISelectable selectable)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
            throw new System.NotImplementedException();
        }

        public void ClearSelection()
        {
            throw new System.NotImplementedException();
        }
    }
}