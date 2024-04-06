using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

namespace Timeline.Editor
{
    public static class EditorCoroutineHelper
    {
        public static EditorCoroutine Delay(Action callback, float timer)
        {
            return EditorCoroutineUtility.StartCoroutineOwnerless(DelayIEnumerator(callback, timer));
        }

        public static EditorCoroutine WaitWhile(Action callback, Func<bool> func)
        {
            return EditorCoroutineUtility.StartCoroutineOwnerless(WaitWhileIEnumerator(callback, func));
        }

        public static EditorCoroutine WaitOneFrame(Action callback)
        {
            return EditorCoroutineUtility.StartCoroutineOwnerless(WaitOneFrameIEnumerator(callback));
        }
        
        private static IEnumerator DelayIEnumerator(Action callback, float timer)
        {
            yield return new EditorWaitForSeconds(timer);
            callback?.Invoke();
        }

        private static IEnumerator WaitWhileIEnumerator(Action callback, Func<bool> func)
        {
            yield return new WaitWhile(func);
            callback?.Invoke();
        }

        private static IEnumerator WaitOneFrameIEnumerator(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
    }
}