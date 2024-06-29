using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class DropdownMenuManipulator: Clickable
    {
        public DropdownMenuManipulator(Action<DropdownMenu> menuBuilder, MouseButton mouseButton, Action<EventBase> onClick = null): this(menuBuilder, mouseButton, false, onClick)
        {
            
        }
        
        private DropdownMenuManipulator(Action<DropdownMenu> menuBuilder, MouseButton mouseButton, bool showWithMouse, Action<EventBase> onClick = null): base(onClick)
        {
            DropdownMenuHandler mDropdownMenuHandler = new(menuBuilder);

            activators.Clear(); //filter
            activators.Add(new ManipulatorActivationFilter()
            {
                button = mouseButton
            });
            
            clickedWithEventInfo += (e) =>
            {
                if (showWithMouse)
                {
                    mDropdownMenuHandler.ShowMenu(e);
                }
                else
                {
                    mDropdownMenuHandler.ShowMenu(target);
                }
            };
        }
    }
}