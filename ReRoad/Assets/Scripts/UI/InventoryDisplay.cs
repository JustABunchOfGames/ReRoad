using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private List<ResourceDisplay> _displayList;
        [SerializeField] private List<ResourceType> _orderOfDisplay;

        [SerializeField] private Text _inventorySlotText;

        private void Awake()
        {
            for(int i = 0;  i < _orderOfDisplay.Count; i++)
            {
                _displayList[i].Setup(new Resource(_orderOfDisplay[i],0));
            }
        }

        public void UpdateDisplay(Inventory inventory)
        {
            foreach(ResourceDisplay display in _displayList)
            {
                display.ChangeQuantity(0);
            }

            List<Resource> resourceList = inventory.GetInventory();

            foreach (Resource resource in resourceList)
            {
                _displayList[_orderOfDisplay.IndexOf(resource.type)].ChangeQuantity(resource.quantity);
            }

            int usedSlot, maxSlot;
            (usedSlot, maxSlot) = inventory.GetInventorySlot();
            SetInventorySlotText(usedSlot + "/" + maxSlot);
        }

        private void SetInventorySlotText(string text)
        {
            _inventorySlotText.text = text;
        }
    }
}