using _Game.Scripts.Cogs;
using UnityEngine;

public interface ISocketAttached
{
    void OnCogAttached(GameObject cog, CogsType type);
    void OnCogDetached(GameObject cog, CogsType type);
}
