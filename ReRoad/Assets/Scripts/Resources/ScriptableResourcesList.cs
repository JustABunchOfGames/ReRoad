using System.Collections.Generic;
using UnityEngine;

namespace Resources 
{
    [CreateAssetMenu(menuName ="Core/Resource/List")]
    public class ScriptableResourcesList : ScriptableObject
    {
        [SerializeField] private List<ScriptableResource> _resources;

        public ScriptableResource GetScriptableRessource(ResourceType type)
        {
            foreach(ScriptableResource resource in _resources)
            {
                if (resource.IsOfType(type))
                    return resource;
            }
            return null;
        }
    }
}