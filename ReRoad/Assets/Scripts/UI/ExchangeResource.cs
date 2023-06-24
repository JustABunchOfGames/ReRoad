using Resources;
using UnityEngine;

namespace UI
{
    public class ExchangeResource : MonoBehaviour
    {
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private bool _playerToTile;

        public void Exchange()
        {
            _inventoryUI.Give1Resource(_playerToTile, _resourceType);
        }

        public void Fill()
        {
            bool isExchangeOk;
            do
            {
                isExchangeOk = _inventoryUI.Give1Resource(_playerToTile, _resourceType);
            }
            while (isExchangeOk);
        }
    }
}