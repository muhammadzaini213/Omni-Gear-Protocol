using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerSocketBuffTest : BaseTriggerObj, ISocketAttached
{
    // ── ISnapperAttached ──────────────────────────────────────────────
    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[PlayerSocketBuffTest] {cog.name} ({type}) attached to {gameObject.name}");
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[PlayerSocketBuffTest] {cog.name} ({type}) detached from {gameObject.name}");
    }
}
