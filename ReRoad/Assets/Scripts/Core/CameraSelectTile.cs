using UnityEngine;

namespace Core
{
    public class CameraSelectTile : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        // To stop raycasting when we selected a Tile, no need to continue to move the indicator
        private bool _stopRaycasting;

        private void Update()
        {
            // Reset the bool when the player stopped doing what it did
            if (_stopRaycasting && GameStateManager.GetState() == GameState.WaitingPlayer)
                _stopRaycasting = false;

            // Cancel Raycasting
            if (Input.GetButtonDown("Cancel"))
            {
                _stopRaycasting = true;
                GameStateManager.ChangeState();
            }

            // Indicate to the tile we're on that it's highlighted / selected
            if (!_stopRaycasting && GameStateManager.GetState() == GameState.SelectingMove)
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    ISelectable selectable = hit.collider.GetComponent<ISelectable>();
                    if (selectable != null)
                    {
                        selectable.OnHighlight();

                        if (Input.GetButtonDown("Select"))
                        {
                            selectable.OnSelect();
                            _stopRaycasting = true;
                        }
                    }
                }
            }
        }
    }
}