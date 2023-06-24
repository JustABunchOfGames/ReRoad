using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public enum GameState
    {
        TileSetup,
        PlayerSetup,
        UISetup,
        WaitingPlayer,
        SelectingMove,
        Moving,
        Harvest,
        Build,
        Craft
    }

    public class GameStateManager : MonoBehaviour
    {
        // Beginning with the map setup
        [SerializeField] private static GameState _state = GameState.TileSetup;

        // If it's called with blank, just reset the state
        // If it's called with a state, set it
        public static void ChangeState(GameState state = GameState.WaitingPlayer)
        {
            // TileSetup -> PlayerSetup -> UISetup -> WaitingPlayer

            if (_state == GameState.TileSetup)
                _state = GameState.PlayerSetup;

            else if (_state == GameState.PlayerSetup)
                _state = GameState.UISetup;

            else
                _state = state;
        }

        // Called from Buttons
        public static void ChangeState(ButtonState buttonState)
        {
            ChangeState(buttonState.state);
        }

        public static GameState GetState()
        {
            return _state;
        }
    }
}