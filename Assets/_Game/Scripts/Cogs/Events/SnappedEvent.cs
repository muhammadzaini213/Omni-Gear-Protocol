using System;
using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class SnappedEvent: MonoBehaviour
    {
        public static Action OnSnappedEvent;

        public static void Subscribe(Action action)
        {
            OnSnappedEvent += action;
        }

        public static void Unsubscribe(Action action)
        {
            OnSnappedEvent -= action;
        }

        public static void OnCogSnappedEvent()
        {
            OnSnappedEvent?.Invoke();
        }
        
    }
}