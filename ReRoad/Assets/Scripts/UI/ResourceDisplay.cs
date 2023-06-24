using Resources;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.DefaultControls;

namespace UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private ScriptableResourceList _resourceIconList;

        [SerializeField] private Text _quantityText;
        [SerializeField] private Image _resourceImage;

        public void Setup(Resource resource)
        {
            _quantityText.text = resource.quantity.ToString();
            _resourceImage.sprite = _resourceIconList.GetIcon(resource.type);
        }

        public void ChangeQuantity(int quantity)
        {
            _quantityText.text = quantity.ToString();
        }
    }
}