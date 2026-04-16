using System;
using UnityEngine;

namespace _Game.Scripts.Cogs
{
    [CreateAssetMenu(fileName = "CogSnapChannel", menuName = "Cogs/Snap Channel")]
    public class CogSnapChannel : ScriptableObject
    {
        public event Action<GameObject, CogsType> OnCogSnapped;
        public event Action<GameObject, CogsType> OnCogUnsnapped;

        public void RaiseSnapped(GameObject cog, CogsType type) =>
            OnCogSnapped?.Invoke(cog, type);

        public void RaiseUnsnapped(GameObject cog, CogsType type) =>
            OnCogUnsnapped?.Invoke(cog, type);
    }
}