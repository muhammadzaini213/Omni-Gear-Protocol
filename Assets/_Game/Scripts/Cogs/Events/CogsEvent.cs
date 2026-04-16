// CogsEvent.cs

using _Game.Scripts.Cogs;

public static class CogsEvent
{
    public delegate void OnCogAttached(CogsType cogType);
    public static event OnCogAttached CogAttached;
    
    public static void BroadcastCogAttached(CogsType cogType)
    {
        CogAttached?.Invoke(cogType);
    }
}