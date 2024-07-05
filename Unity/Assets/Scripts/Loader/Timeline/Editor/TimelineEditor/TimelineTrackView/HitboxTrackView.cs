using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class HitboxTrackView: TimelineTrackView
    {
        private readonly List<HitboxMarkerView> markerViews = new();
        private BBHitboxTrack Track;

        public override void Init(RuntimeTrack track)
        {
            RuntimeTrack = track;
            Track = RuntimeTrack.Track as BBHitboxTrack;

            int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (var pair in Track.KeyframeDict)
            {
                HitboxMarkerView markerView = new();
                markerView.Init(this, pair.Value);

                markerViews.Add(markerView);
                Add(markerView);
                FieldView.SelectionElements.Add(markerView); // selection
            }
        }

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            var trackScrollView = FieldView.Q<ScrollView>("track-scroll");
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x + trackScrollView.scrollOffset.x);
            foreach (HitboxMarkerView markerView in markerViews)
            {
                if (!markerView.InMiddle(targetFrame))
                {
                    continue;
                }

                markerView.OnPointerDown(evt);
            }

            //右键
            if (evt.button == 1)
            {
                // Open menu builder
                evt.StopImmediatePropagation();
            }
        }

        protected override void OnPointerMove(PointerMoveEvent evt)
        {
        }
    }
}