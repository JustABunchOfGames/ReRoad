using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ResourceDisplay : MonoBehaviour
    {
        [Serializable]
        public class Display
        {
            public ResourceType resourceType;
            public int quantity;
            public Text resourceText;

            public void UpdateText()
            {
                resourceText.text = quantity.ToString();
            }
        }

        [SerializeField] private List<Display> _displayList;

        public void SetQuantity(Resource resource)
        {
            foreach (Display display in _displayList)
            {
                if (display.resourceType == resource.type)
                {
                    display.quantity = resource.quantity;
                    display.UpdateText();
                    return;
                }
            }
        }

        public void AddQuantity(Resource resource)
        {
            foreach (Display display in _displayList)
            {
                if (display.resourceType == resource.type)
                {
                    display.quantity += resource.quantity;
                    display.UpdateText();
                    return;
                }
            }
        }

        public void SubstractQuantity(Resource resource)
        {
            foreach (Display display in _displayList)
            {
                if (display.resourceType == resource.type)
                {
                    display.quantity -= resource.quantity;
                    display.UpdateText();
                    return;
                }
            }
        }
    }
}