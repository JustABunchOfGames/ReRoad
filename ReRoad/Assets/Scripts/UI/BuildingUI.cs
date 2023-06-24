using Build;
using Core;
using Resources;
using System.Collections.Generic;
using UnityEngine;

namespace UI 
{
    public class BuildingUI : MonoBehaviour
    {
        private Player.Player _player;

        [SerializeField] private ScriptableBuildCost _scriptableBuildCost;

        [SerializeField] private ConfirmBox _confirmBox;

        [Header("TextForOutpost")]
        [TextArea]
        [SerializeField] private string _textForOutpost;

        public void Setup(Player.Player player)
        {
            _player = player;
        }

        public void ProposeToBuild()
        {
            // Check the cost of the build
            List<Resource> buildCost = _scriptableBuildCost.GetOupostBuildCost();

            // Get the resources the player & the tile has
            Inventory inventory = Inventory.ConcatInventories(_player.inventory, _player.currentTile.inventory);

            bool canPayCost = inventory.CanPayCost(buildCost);

            _confirmBox.gameObject.SetActive(true);
            _confirmBox.SetConfirmBox(_textForOutpost, buildCost, canPayCost);
        }

        public void Build(bool confirm)
        {
            if (confirm)
            {
                
            }

            GameStateManager.ChangeState();
        }
    }
}