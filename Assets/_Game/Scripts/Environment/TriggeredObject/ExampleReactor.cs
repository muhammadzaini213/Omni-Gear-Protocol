using _Game.Scripts.Cogs;
using UnityEngine;


// THIS IS AN EXAMPLE REACTOR, NOT A REAL COMPONENT, 
// YOU COULD COPY AND PASTE THIS TO YOUR REAL REACTOR COMPONENTS, 
// OR JUST USE THIS AS A REFERENCE

public class ExampleReactor : MonoBehaviour, ISocketAttached
{
    [SerializeField] private CogSnapChannel snapChannel;

    private void OnEnable()
    {
        snapChannel.OnCogSnapped += HandleSnapped;
        snapChannel.OnCogUnsnapped += HandleUnsnapped;
    }

    private void OnDisable()
    {
        snapChannel.OnCogSnapped -= HandleSnapped;
        snapChannel.OnCogUnsnapped -= HandleUnsnapped;
    }

    private void HandleSnapped(GameObject cog, CogsType type)
    {
        OnCogAttached(cog, type);
    }

    private void HandleUnsnapped(GameObject cog, CogsType type)
    {
        OnCogDetached(cog, type);
    }


    // ── ISnapperAttached ──────────────────────────────────────────────
    public void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[ExampleReactor] {cog.name} ({type}) attached to {gameObject.name}");
    }

    public void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[ExampleReactor] {cog.name} ({type}) detached from {gameObject.name}");
    }
}
