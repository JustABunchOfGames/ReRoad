using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Resource/New")]
    public class ScriptableResource : ScriptableObject
    {
        [SerializeField] private ResourceType _type;

        [SerializeField] private Sprite icon;
        [SerializeField] private int id;

        public ResourceType GetResourceType()
        {
            return _type;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public int GetID()
        {
            return id;
        }
    }
}