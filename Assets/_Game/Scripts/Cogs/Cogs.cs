using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;

        public void Awake()
        {
            if (cogType == CogsType.Small)
            {
                gameObject.AddComponent<SmallCogs>();
            }

            if (cogType == CogsType.Medium)
            {
                gameObject.AddComponent<MediumCogs>();
            }

            if (cogType == CogsType.Large)
            {
                gameObject.AddComponent<LargeCogs>();
            }
        }
        
    }

    public enum CogsType
    {
        Small,
        Medium,
        Large   
    }
}