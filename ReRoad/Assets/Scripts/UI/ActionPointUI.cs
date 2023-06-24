using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionPointUI : MonoBehaviour
    {
        [Header("ActionPointList")]
        [SerializeField] private List<GameObject> _actionPointList;

        [Header("ActionPointUsedInMovement")]
        [TextArea]
        [SerializeField] private string _textForActionPointUsedByMovement = "Action Point used by selected action :";
        [SerializeField] private Text _actionPointUsedByMovement;

        // Preview of ActionPoint for moving
        public void ShowActionPointUsedByMovement(bool show, int actionPoint)
        {
            _actionPointUsedByMovement.text = _textForActionPointUsedByMovement + actionPoint.ToString();
            _actionPointUsedByMovement.gameObject.SetActive(show);
        }

        public void UpdateActionPoint(int actionPointUsed)
        {
            if (actionPointUsed == 0)
            {
                foreach (GameObject actionPoint in _actionPointList)
                {
                    actionPoint.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject actionPoint in _actionPointList)
                {
                    if (actionPointUsed > 0)
                    {
                        actionPoint.SetActive(false);
                        actionPointUsed--;
                    }
                }
            }
        }
    }
}