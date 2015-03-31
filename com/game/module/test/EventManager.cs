using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace com.game.module.test
{
    public static class EventManager
    {
        public interface EventProcessInterface
        {
            void ProcessEvent(int evt, object param);
        }
        public interface IEventUnregisterable : EventManager.EventProcessInterface
        {
            bool HasRegisterEvent
            {
                get;
                set;
            }
        }
        private class EventListener
        {
            public List<EventManager.EventProcessInterface> listeners;
            public EventListener()
            {
                this.listeners = new List<EventManager.EventProcessInterface>();
            }
        }
        private static List<int> _eventQueue = new List<int>();
        private static List<object> _paramQueue = new List<object>();
        private static Dictionary<int, EventManager.EventListener> _eventList = new Dictionary<int, EventManager.EventListener>();
        private static string LAG_KEY = "event_manager_update()";
        private static string TMP_KEY = string.Empty;
        public static void Init()
        {
            EventManager._eventList = new Dictionary<int, EventManager.EventListener>();
            EventManager._eventQueue = new List<int>();
        }
        public static int GetListenersCount()
        {
            int num = 0;
            foreach (KeyValuePair<int, EventManager.EventListener> current in EventManager._eventList)
            {
                num += current.Value.listeners.Count;
            }
            return num;
        }
        public static string DumpListeners()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("EventManager.DumpListeners\n", new object[0]);
            foreach (KeyValuePair<int, EventManager.EventListener> current in EventManager._eventList)
            {
                stringBuilder.AppendFormat("  event: {0}, count: {1}\n", current.Key, current.Value.listeners.Count);
                foreach (EventManager.EventProcessInterface current2 in current.Value.listeners)
                {
                    stringBuilder.AppendFormat("    {0}\n", current2);
                }
            }
            return stringBuilder.ToString();
        }
        public static void UnRegisterAll(EventManager.IEventUnregisterable processor)
        {
            if (!processor.HasRegisterEvent)
            {
                return;
            }
            foreach (KeyValuePair<int, EventManager.EventListener> current in EventManager._eventList)
            {
                List<EventManager.EventProcessInterface> listeners = current.Value.listeners;
                if (listeners.Contains(processor))
                {
                    List<EventManager.EventProcessInterface> list = new List<EventManager.EventProcessInterface>();
                    foreach (EventManager.EventProcessInterface current2 in listeners)
                    {
                        if (current2 != processor)
                        {
                            list.Add(current2);
                        }
                    }
                    current.Value.listeners = list;
                }
            }
        }
        public static void RegisterEvent(int evt, EventManager.EventProcessInterface processor)
        {
            EventManager.EventListener eventListener = null;
            if (!EventManager._eventList.TryGetValue(evt, out eventListener))
            {
                eventListener = new EventManager.EventListener();
                EventManager._eventList.Add(evt, eventListener);
            }
            if (eventListener.listeners.Contains(processor))
            {
                Debug.LogError("事件重复添加!");
                return;
            }
            eventListener.listeners.Add(processor);
            EventManager.IEventUnregisterable eventUnregisterable = processor as EventManager.IEventUnregisterable;
            if (eventUnregisterable != null)
            {
                eventUnregisterable.HasRegisterEvent = true;
            }
        }
        public static void UnRegisterEvent(int evt, EventManager.EventProcessInterface processor)
        {
            EventManager.EventListener eventListener = null;
            if (EventManager._eventList.TryGetValue(evt, out eventListener))
            {
                eventListener.listeners.Remove(processor);
            }
        }
        public static void SendEvent(int evt, object param)
        {
            if (!EventManager._eventList.ContainsKey(evt))
            {
                return;
            }
            EventManager.EventProcessInterface[] array = EventManager._eventList[evt].listeners.ToArray();
            EventManager.EventProcessInterface[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                EventManager.EventProcessInterface eventProcessInterface = array2[i];
                eventProcessInterface.ProcessEvent(evt, param);
            }
        }
        public static void SendEvent(int evt)
        {
            EventManager.SendEvent(evt, null);
        }
        public static void PostEvent(int evt, object param)
        {
            if (!EventManager._eventList.ContainsKey(evt))
            {
                return;
            }
            EventManager._eventQueue.Add(evt);
            EventManager._paramQueue.Add(param);
        }
        public static void PostEvent(int evt)
        {
            EventManager.PostEvent(evt, null);
        }
        public static void PostCombineEvent(int evt, object param)
        {
            if (!EventManager._eventList.ContainsKey(evt))
            {
                return;
            }
            int num = EventManager._eventQueue.IndexOf(evt);
            if (num >= 0 && EventManager._paramQueue[num] == param)
            {
                return;
            }
            EventManager._eventQueue.Add(evt);
            EventManager._paramQueue.Add(param);
        }
        public static void Update()
        {
            if (EventManager._eventQueue.Count == 0)
            {
                return;
            }
            List<int> eventQueue = EventManager._eventQueue;
            List<object> paramQueue = EventManager._paramQueue;
            EventManager._eventQueue = new List<int>();
            EventManager._paramQueue = new List<object>();
            for (int i = 0; i < eventQueue.Count; i++)
            {
                int num = eventQueue[i];
                object param = paramQueue[i];
                EventManager.EventProcessInterface[] array = EventManager._eventList[num].listeners.ToArray();
                EventManager.EventProcessInterface[] array2 = array;
                for (int j = 0; j < array2.Length; j++)
                {
                    EventManager.EventProcessInterface eventProcessInterface = array2[j];
                    try
                    {
                        //LagDebugControl.BeginDebug(EventManager.LAG_KEY, num, LagDebugControl.LAG_TIME);
                        eventProcessInterface.ProcessEvent(num, param);
                        //LagDebugControl.EndPoint(EventManager.LAG_KEY, num);
                    }
                    catch (Exception ex)
                    {
                        string str = string.Empty;
                        if (ex.InnerException != null)
                        {
                            str = ex.InnerException.Message + "\n" + ex.InnerException.StackTrace;
                        }
                        else
                        {
                            str = ex.Message + "\n" + ex.StackTrace;
                        }
                        //Utility.AddException(str);
                    }
                }
            }
        }
    }
}
