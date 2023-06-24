using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Resource/List")]
    public class ScriptableResourceList : ScriptableObject
    {
        [SerializeField] private List<ScriptableResource> _resourceList;

        public Sprite GetIcon(ResourceType resourceType)
        {
            foreach(ScriptableResource resource in _resourceList)
            {
                if (resource.GetResourceType() == resourceType)
                    return resource.GetIcon();
            }
            return null;
        }
    }
}
