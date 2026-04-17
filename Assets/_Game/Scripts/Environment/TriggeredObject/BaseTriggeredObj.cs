using _Game.Scripts.Cogs;
using UnityEngine;

public abstract class BaseTriggerObj : MonoBehaviour, ISocketAttached
{
    [Header("Settings")]
    [SerializeField] protected CogSnapChannel snapChannel;

    protected virtual void OnEnable()
    {
        if (snapChannel == null)
        {
            Debug.LogWarning($"Snap Channel is null on {gameObject.name}");
            return;
        }

        snapChannel.OnCogSnapped += HandleSnapped;
        snapChannel.OnCogUnsnapped += HandleUnsnapped;
    }

    protected virtual void OnDisable()
    {
        if (snapChannel == null) return;

        snapChannel.OnCogSnapped -= HandleSnapped;
        snapChannel.OnCogUnsnapped -= HandleUnsnapped;
    }

    private void HandleSnapped(GameObject cog, CogsType type) => OnCogAttached(cog, type);
    private void HandleUnsnapped(GameObject cog, CogsType type) => OnCogDetached(cog, type);

    // ── Abstract Methods ──────────────────────────────────────────────
    // The Children of this class MUST implement these methods, to react to the snap events
    public abstract void OnCogAttached(GameObject cog, CogsType type);
    public abstract void OnCogDetached(GameObject cog, CogsType type);
}