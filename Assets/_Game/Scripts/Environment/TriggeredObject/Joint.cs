using _Game.Scripts.Cogs;
using UnityEngine;

public class Joint : BaseTriggerObj
{
    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Cart] {cog.name} ({type}) attached to {gameObject.name}");
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Cart] {cog.name} ({type}) detached from {gameObject.name}");
    }
}