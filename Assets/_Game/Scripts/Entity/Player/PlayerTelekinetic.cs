using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerTelekinetic : BaseTriggerObj, ISocketAttached
{
    [SerializeField] private float timeBeforeDeactivate = 5f;
    private CogsDrag[] allCogs;
    void Start()
    {
        allCogs = FindObjectsByType<CogsDrag>(FindObjectsSortMode.None);
    }

    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        ActivateDrag();
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Invoke(nameof(DeactivateDrag), timeBeforeDeactivate);
    }

    private void ActivateDrag()
    {
        foreach (var c in allCogs)
        {
            c.gameObject.tag = "Cogs";
        }
    }

    private void DeactivateDrag()
    {
        foreach (var c in allCogs)
        {
            if (c == null) continue;

            if (c.onDrag)
            {
                c.ForceRelease();
            }

            c.gameObject.tag = "CogsDisabled";
        }
    }
}