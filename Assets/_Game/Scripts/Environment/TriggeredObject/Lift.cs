using _Game.Scripts.Cogs;
using UnityEngine;

public class Lift : BaseTriggerObj
{
    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Lift] {cog.name} ({type}) attached to {gameObject.name}");
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Lift] {cog.name} ({type}) detached from {gameObject.name}");
    }
}