using System;

namespace Player
{
    [Serializable]
    public class SurvivalStatus
    {
        public int maxHealth;
        public int health;

        public int maxStamina;
        public int stamina;

        public int maxFood;
        public int food;

        public int maxWater;
        public int water;

        public SurvivalStatus(int maxHealth, int maxStamina, int maxFood, int maxWater)
        {
            this.maxHealth = maxHealth;
            this.health = maxHealth;

            this.maxStamina = maxStamina;
            this.stamina = maxStamina;

            this.maxFood = maxFood;
            this.food = maxFood;

            this.maxWater = maxWater;
            this.water = maxWater;
        }
    }
}