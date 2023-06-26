using UnityEngine;

namespace Core
{
    public class CameraSelectTile : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        // Selecting in progress
        private bool _isSelecting;

        private void Update()
        {

            // Indicate to the tile we're on that it's highlighted / selected
            if (_isSelecting)
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    ISelectable selectable = hit.collider.GetComponent<ISelectable>();
                    if (selectable != null)
                    {
                        // Say to the tile it is highlighted
                        selectable.OnHighlight();

                        if (Input.GetButtonDown("Select"))
                        {
                            // Say to the tile it is selected
                            selectable.OnSelect();
                            
                            // Finished selecting
                            _isSelecting = false;
                        }
                    }
                }
            }
        }

        public void StartRaycast()
        {
            _isSelecting = true;
        }

        public void StopRaycast()
        {
            _isSelecting = false;
        }
    }
}