using UnityEngine;

namespace Core
{
    public enum RessourceType
    {
        Wood,
        Stone,
        Concrete,
        Metal,
        Electronic
    }

    public class Ressources
    {
        private RessourceType _type;

        public int quantity;

        public Ressources(RessourceType type){
            _type = type;
        }
    }
}