using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.game.module.test
{
    public class CoroutineManager
    {
        private static readonly List<Task> taskList = new List<Task>();
        private static CoroutineBehaviour behaviour;

        public static Task StartCoroutine(IEnumerator routine, bool autoStart = true)
        {
            var t = new Task(routine, autoStart);
            taskList.Add(t);
            return t;
        }

        public static void StopCoroutine(Task t)
        {
            if (taskList.Contains(t))
            {
                t.Stop();
                taskList.Remove(t);
            }
        }

        public static void PauseCoroutine(Task t)
        {
            if (taskList.Contains(t))
            {
                t.Pause();
            }
        }

        public static void ResumeCoroutine(Task t)
        {
            if (taskList.Contains(t))
            {
                t.Unpause();
            }
        }

        public static void PauseAllCoroutine()
        {
            foreach (Task t in taskList)
            {
                t.Pause();
            }
        }

        public static void ResumeAllCoroutine()
        {
            foreach (Task t in taskList)
            {
                t.Unpause();
            }
        }

        public static void StopAllCoroutine()
        {
            foreach (Task t in taskList)
            {
                t.Stop();
            }
            taskList.Clear();
        }


        public static Coroutine StartCoroutine(string methodName, object value = null)
        {
            if (behaviour == null)
            {
                var coroutineManager = new GameObject("CoroutineManager");
                Object.DontDestroyOnLoad(coroutineManager);
                behaviour = coroutineManager.AddComponent<CoroutineBehaviour>();
            }
            return behaviour.StartCoroutine(methodName, value);
        }

        public static void StopCoroutine(string methodName)
        {
            if (behaviour == null) { UnityEngine.Debug.Log("behaviour is null"); return; }
            behaviour.StopCoroutine(methodName);
        }
    }

    public class CoroutineBehaviour : MonoBehaviour
    {
    }
}