using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (DlgStorage))]
    [FriendOf(typeof (DialogueStorageManager))]
    public static class DlgStorageSystem
    {
        public static void RegisterUIEvent(this DlgStorage self)
        {
        }

        public static void ShowWindow(this DlgStorage self, Entity contextData = null)
        {
        }

        public static void Refresh(this DlgStorage self)
        {
            for (int i = 0; i < DialogueStorageManager.Instance.shots.Length; i++)
            {
                DialogueStorage storage = DialogueStorageManager.Instance.GetByIndex(i);
                
            }
        }
    }
}