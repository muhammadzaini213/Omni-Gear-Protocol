using _Game.Scripts.Cogs;
using UnityEngine;

public class Gate : BaseTriggerObj
{
    private Animator _animator;
    private Collider2D _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        if (_collider == null) _collider = GetComponentInChildren<Collider2D>();
    }

    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        if (_animator != null) _animator.Play("Gate_Open");
        if (_collider != null) _collider.enabled = false;
        
        Debug.Log($"Gate Opened with {type} Cog");
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        if (_animator != null) _animator.Play("Gate_Close");
        if (_collider != null) _collider.enabled = true;

        Debug.Log($"Gate Closed - Cog {type} removed");
    }
}