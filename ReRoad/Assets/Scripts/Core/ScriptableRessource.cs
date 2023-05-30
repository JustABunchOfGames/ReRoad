using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName ="Core/Ressource/New")]
    public class ScriptableRessource : ScriptableObject
    {
        [SerializeField] private RessourceType _type;

        public GameObject icon;
        public int id;

        public bool IsOfType(RessourceType type)
        {
            return _type == type;
        }

        public GameObject GetIcon(RessourceType type)
        {
            if (_type == type)
                return icon;

            return null;
        }

        public int GetID(RessourceType type)
        {
            if (_type == type) 
                return id;

            return -1; 
        }
    }
}