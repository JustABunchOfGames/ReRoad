using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SurvivalUI : MonoBehaviour
    {
        // Health
        [SerializeField] private Image _healthImage;
        [SerializeField] private Text _healthText;
        [Space(10)]

        // Stamina
        [SerializeField] private Image _staminaImage;
        [SerializeField] private Text _staminaText;
        [Space(10)]

        // Food
        [SerializeField] private Image _foodImage;
        [SerializeField] private Text _foodText;
        [Space(10)]

        // Water
        [SerializeField] private Image _waterImage;
        [SerializeField] private Text _waterText;

        public void UpdateSurvivalStatus(SurvivalStatus survivalStatus)
        {
            // Health
            UpdateStatus(_healthImage, _healthText, survivalStatus.health, survivalStatus.maxHealth);

            // Stamina
            UpdateStatus(_staminaImage, _staminaText, survivalStatus.stamina, survivalStatus.maxStamina);

            // Food
            UpdateStatus(_foodImage, _foodText, survivalStatus.food, survivalStatus.maxFood);

            // Water
            UpdateStatus(_waterImage, _waterText, survivalStatus.water, survivalStatus.maxWater);
        }

        private void UpdateStatus(Image image, Text text, int currentValue, int maxValue)
        {
            image.fillAmount = (float)currentValue / (float)maxValue;
            text.text = currentValue.ToString();
        }
    }
}