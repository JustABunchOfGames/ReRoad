using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(menuName ="Core/Ressource/List")]
    public class ScriptableRessourcesList : ScriptableObject
    {
        [SerializeField] private List<ScriptableRessource> _ressources;

        public ScriptableRessource GetScriptableRessource(RessourceType type)
        {
            foreach(ScriptableRessource ressource in _ressources)
            {
                if (ressource.IsOfType(type))
                    return ressource;
            }
            return null;
        }
    }
}