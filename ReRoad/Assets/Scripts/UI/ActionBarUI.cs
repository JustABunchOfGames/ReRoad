using Core;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ActionBarUI : MonoBehaviour
    {
        private Dictionary<GameState, ButtonState> _buttonStateDictionary;

        private void Start()
        {
            _buttonStateDictionary = new Dictionary<GameState, ButtonState>();

            // Save all ButtonState to activate/de-activate them
            foreach (Transform t in transform)
            {
                ButtonState buttonState = t.GetComponent<ButtonState>();
                
                if (buttonState != null)
                    _buttonStateDictionary.Add(buttonState.state, buttonState);
            }
        }

        public void DeactivateAll()
        {
            foreach(ButtonState buttonState in _buttonStateDictionary.Values)
            {
                buttonState.Activate(false);
            }
        }

        public void ActivateStateButton(GameState state, bool active)
        {
            _buttonStateDictionary[state].Activate(active);
        }
    }
}