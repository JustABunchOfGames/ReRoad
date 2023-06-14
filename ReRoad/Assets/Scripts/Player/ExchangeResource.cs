using Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ExchangeResource : MonoBehaviour
    {
        [SerializeField] private PlayerUI _playerUI;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private bool _playerToTile;

        public void Exchange()
        {
            _playerUI.ExchangeResource(_playerToTile, _resourceType);
        }

        public void Fill()
        {
            _playerUI.FillResource(_playerToTile, _resourceType);
        }
    }
}