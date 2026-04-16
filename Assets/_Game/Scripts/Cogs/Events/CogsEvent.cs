// CogsEvent.cs

using _Game.Scripts.Cogs;
using UnityEngine;

public static class CogsEvent
{
    public delegate void OnCogAttached(GameObject obj, CogsType cogType);
    public static event OnCogAttached CogAttached;
    
    public static void BroadcastCogAttached(GameObject obj,CogsType cogType)
    {
        CogAttached?.Invoke(obj, cogType);
    }
}