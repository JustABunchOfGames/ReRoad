using Resources;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConfirmBox : MonoBehaviour
    {
        [Header("CoreText")]
        [SerializeField] private Text _text;

        [Header("Cost")]
        [SerializeField] private GameObject _costPlaceholder;
        [SerializeField] private Vector3 _startingPosition;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private ResourceDisplay _resourceDisplayPrefab;

        [Header("Buttons")]
        [SerializeField] private Image _hiddingConfirmImage;

        public void SetConfirmBox(string text, List<Resource> cost, bool canConfirm)
        {
            // Core Text
            _text.text = text;

            for(int i = 0; i < cost.Count; i++)
            {
                ResourceDisplay resourceDisplay = Instantiate(_resourceDisplayPrefab, _costPlaceholder.transform);
                resourceDisplay.GetComponent<RectTransform>().anchoredPosition3D = _startingPosition + (_offset * i);
                resourceDisplay.Setup(cost[i]);
            }

            // Activate confirm button if the cost can be paid
            _hiddingConfirmImage.gameObject.SetActive(!canConfirm);
        }
    }
}