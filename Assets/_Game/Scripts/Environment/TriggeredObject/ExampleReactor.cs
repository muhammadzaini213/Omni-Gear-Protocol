using _Game.Scripts.Cogs;
using UnityEngine;


// THIS IS AN EXAMPLE REACTOR, NOT A REAL COMPONENT, 
// YOU COULD COPY AND PASTE THIS TO YOUR REAL REACTOR COMPONENTS, 
// OR JUST USE THIS AS A REFERENCE

public class ExampleReactor : BaseTriggerObj, ISocketAttached
{

    // ── ISnapperAttached ──────────────────────────────────────────────
    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[ExampleReactor] {cog.name} ({type}) attached to {gameObject.name}");
    }


    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[ExampleReactor] {cog.name} ({type}) detached from {gameObject.name}");
    }

}
