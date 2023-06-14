using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName ="Core/Resource/New")]
    public class ScriptableResource : ScriptableObject
    {
        [SerializeField] private ResourceType _type;

        public GameObject icon;
        public int id;

        public bool IsOfType(ResourceType type)
        {
            return _type == type;
        }

        public GameObject GetIcon(ResourceType type)
        {
            if (_type == type)
                return icon;

            return null;
        }

        public int GetID(ResourceType type)
        {
            if (_type == type) 
                return id;

            return -1; 
        }
    }
}