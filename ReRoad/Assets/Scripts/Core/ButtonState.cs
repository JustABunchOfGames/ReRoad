using UnityEngine;

namespace Core {
    public class ButtonState : MonoBehaviour
    {
        public GameState state;

        [SerializeField] private GameObject _hiddingImage;

        public void Activate(bool active)
        {
            _hiddingImage.SetActive(!active);
        }

        public bool isActive()
        {
            return !_hiddingImage.activeSelf;
        }
    }
}