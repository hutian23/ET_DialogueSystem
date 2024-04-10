using System;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class DropdownMenuManipulator: Clickable
    {
        private DropdownMenuHandler m_DropdownMenuHandler;
        private bool m_ShowWithMouse;

        public DropdownMenuManipulator(Action<DropdownMenu> menuBuilder, MouseButton mouseButton, Action<EventBase> onClick = null): this(menuBuilder, mouseButton, false, onClick)
        {
            
        }
        
        public DropdownMenuManipulator(Action<DropdownMenu> menuBuilder, MouseButton mouseButton, bool showWithMouse, Action<EventBase> onClick = null): base(onClick)
        {
            m_DropdownMenuHandler = new DropdownMenuHandler(menuBuilder);
            m_ShowWithMouse = showWithMouse;
            
            activators.Clear();
            activators.Add(new ManipulatorActivationFilter()
            {
                button = mouseButton
            });
            clickedWithEventInfo += (e) =>
            {
                if (m_ShowWithMouse)
                {
                    m_DropdownMenuHandler.ShowMenu(e);
                }
                else
                {
                    m_DropdownMenuHandler.ShowMenu(target);
                }
            };
        }
    }
}