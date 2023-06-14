using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [Serializable]
    public enum GameState
    {
        TileSetup,
        PlayerSetup,
        WaitingPlayer,
        Move,
        Harvest,
        Build,
        Craft
    }

    public class GameStateManager : MonoBehaviour
    {
        // Beginning with the map setup
        private GameState _state = GameState.TileSetup;

        // If it's called with blank, just reset the state
        // If it's called with a state, set it
        public void ChangeState(GameState state = GameState.WaitingPlayer)
        {
            if (_state == GameState.TileSetup)
            {
                _state = GameState.PlayerSetup;
            }
            else
            {
                _state = state;
            }
        }

        // Called from Buttons
        public void ChangeState(ButtonState buttonState)
        {
            ChangeState(buttonState.state);
        }

        public GameState GetState()
        {
            return _state;
        }
    }
}