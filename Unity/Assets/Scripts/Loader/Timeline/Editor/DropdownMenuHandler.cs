using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    //下拉菜单
    public class DropdownMenuHandler
    {
        private readonly Action<DropdownMenu> m_MenuBuilder;

        public DropdownMenuHandler(Action<DropdownMenu> menuBuilder)
        {
            m_MenuBuilder = menuBuilder;
        }

        public void ShowMenu(EventBase eventBase)
        {
            DropdownMenu dropdownMenu = new();
            m_MenuBuilder?.Invoke(dropdownMenu);
            if (!dropdownMenu.MenuItems().Any())
            {
                return;
            }

            DoDisplayEditorMenu(dropdownMenu, eventBase);
        }

        public void ShowMenu(VisualElement target)
        {
            DropdownMenu dropdownMenu = new();
            m_MenuBuilder?.Invoke(dropdownMenu);
            if (!dropdownMenu.MenuItems().Any())
            {
                return;
            }

            if (target != null)
            {
                Rect worldBound = target.worldBound;
                if (worldBound.x <= 0f)
                {
                    worldBound.x = 1f;
                }

                DoDisplayEditorMenu(dropdownMenu, worldBound);
            }
        }

        private GenericMenu PrepareMenu(DropdownMenu menu, EventBase triggerEvent)
        {
            menu.PrepareForDisplay(triggerEvent);
            GenericMenu genericMenu = new();
            foreach (DropdownMenuItem item in menu.MenuItems())
            {
                if (item is DropdownMenuAction action)
                {
                    if ((action.status & DropdownMenuAction.Status.Hidden) == DropdownMenuAction.Status.Hidden ||
                        action.status == DropdownMenuAction.Status.None)
                    {
                        continue;
                    }

                    bool on = (action.status & DropdownMenuAction.Status.Checked) == DropdownMenuAction.Status.Checked;
                    if ((action.status & DropdownMenuAction.Status.Disabled) == DropdownMenuAction.Status.Disabled)
                    {
                        genericMenu.AddDisabledItem(new GUIContent(action.name), on);
                    }

                    genericMenu.AddItem(new GUIContent(action.name), on, delegate { action.Execute(); });
                }
                else
                {
                    if (item is DropdownMenuSeparator dropdownMenuSeparator)
                    {
                        genericMenu.AddSeparator(dropdownMenuSeparator.subMenuPath);
                    }
                }
            }

            return genericMenu;
        }

        private void DoDisplayEditorMenu(DropdownMenu menu, Rect rect)
        {
            PrepareMenu(menu, null).DropDown(rect);
        }

        private void DoDisplayEditorMenu(DropdownMenu menu, EventBase triggerEvent)
        {
            GenericMenu genericMenu = PrepareMenu(menu, triggerEvent);
            Vector2 position = Vector2.zero;
            if (triggerEvent is IMouseEvent m_evt)
            {
                position = m_evt.mousePosition;
            }
            else if (triggerEvent is IPointerEvent p_evt)
            {
                position = p_evt.position;
            }
            else if (triggerEvent.target is VisualElement v)
            {
                position = v.layout.center;
            }

            genericMenu.DropDown(new Rect(position, Vector2.zero));
        }
    }
}