using System;

namespace Resources
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Concrete,
        Metal,
        Electronic
    }

    [Serializable]
    public class Resource
    {
        public ResourceType type;

        public int quantity;

        public Resource(ResourceType type, int quantity)
        {
            this.type = type;
            this.quantity = quantity;
        }
    }
}