using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameStateManager _gameStateManager;

        [SerializeField] private GameObject _actionBar;

        private void Update()
        {
            if (!_actionBar.activeSelf && _gameStateManager.GetState() == GameState.WaitingPlayer)
                _actionBar.SetActive(true);
            else if (_actionBar.activeSelf && _gameStateManager.GetState() != GameState.WaitingPlayer)
                _actionBar.SetActive(false);
        }
    }
}