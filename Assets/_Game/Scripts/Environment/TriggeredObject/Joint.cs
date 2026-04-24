using _Game.Scripts.Cogs;
using UnityEngine;
using UnityEngine.Serialization;

public class Joint : BaseTriggerObj
{
    [SerializeField] private float targetedrotationDegree;
    [SerializeField] private float initialRotationDegree;
    [SerializeField] float rotationSpeed;
    [SerializeField] private AudioClip jointClip;

    private bool isSocketEmpty = true;
    [FormerlySerializedAs("_transform")] public Transform _rotationPoint;

    private void Update()
    {
        RotateJoint();
    }

    public void RotateJoint()
    {

        float targetZ = isSocketEmpty ? initialRotationDegree : targetedrotationDegree;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZ);

        if (Quaternion.Angle(_rotationPoint.rotation, targetRotation) > 0.1f)
        {
            _rotationPoint.rotation = Quaternion.Slerp(
                _rotationPoint.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
    // ── ISnapperAttached ──────────────────────────────────────────────
    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        isSocketEmpty = false;
        Debug.Log($"[Cart] {cog.name} ({type}) attached to {gameObject.name}");
        SfxPlayer.Instance.PlayEnvironmentSfx(jointClip, volume: 0.5f);
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        isSocketEmpty = true;
        Debug.Log($"[Cart] {cog.name} ({type}) detached from {gameObject.name}");
        SfxPlayer.Instance.PlayEnvironmentSfx(jointClip, volume: 0.5f);
    }
}