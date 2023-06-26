using Resources;
using UnityEngine;

namespace Action
{
    public class ExchangeResource : MonoBehaviour
    {
        [SerializeField] private ActionManageResource _manageResource;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private bool _playerToTile;
        [SerializeField] private bool _fill;

        public void Exchange()
        {
            _manageResource.ExchangeResource(_playerToTile, _fill, _resourceType);
        }
    }
}