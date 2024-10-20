﻿using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(Background))]
    public static class BackgroundSystem
    {
        public class BackgroundAwakeSystem : AwakeSystem<Background>
        {
            protected override void Awake(Background self)
            {
                ResourcesComponent.Instance.LoadBundle("background.unity3d");
                var prefab = ResourcesComponent.Instance.GetAsset("background.unity3d", "Background") as GameObject;
                self.background = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit);
                self.background.SetActive(false);
            }
        }
            
        public class BackgroundDestroySystem : DestroySystem<Background>
        {
            protected override void Destroy(Background self)
            {
                UnityEngine.Object.Destroy(self.background);
            }
        }
        
        public class BackgroundLoadSystem : LoadSystem<Background>
        {
            protected override void Load(Background self)
            {
                self.Dispose();
            }
        }
        
        public static void ShowBackground(this Background self, Sprite sprite)
        {
            self.background.GetComponent<SpriteRenderer>().sprite = sprite;
            self.background.SetActive(true);
        }

        public static void ShowBackground(this Background self)
        {
            self.background.SetActive(true);
        }

        public static void HideBackground(this Background self)
        {
            self.background.SetActive(false);
        }
    }
}