using Codice.CM.Common;
using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("CameraReference")]
        [SerializeField] private Camera _camera;
        private Transform _cameraTransform;

        [Header("CameraLimits")]
        [SerializeField] private Vector3 _minPosition;
        [SerializeField] private Vector3 _maxPosition;
        [SerializeField] private float _minZoom;
        [SerializeField] private float _maxZoom;

        [Header("CameraSpeed")]
        [SerializeField] private float _normalSpeed;
        [SerializeField] private float _fastSpeed;
        [SerializeField] private float _keyRotationAmount;
        [SerializeField] private float _mouseRotationAmount;
        [SerializeField] private Vector3 _zoomAmount;

        [Header("CameraRigidity")]
        [SerializeField] private float _movementTime;

        // Speed
        private float _movementSpeed;

        // Movement with Key
        private Vector3 _newPosition;
        private Quaternion _newRotation;

        // Zoom
        private Vector3 _newZoom;

        // Movement with Mouse
        private Vector3 _dragStartPosition;
        private Vector3 _drageCurrentPosition;
        Plane _plane;

        // Rotation with Mouse
        private Vector3 _rotateStartPosition;
        private Vector3 _rotateCurrentPosition;

        // Target
        [SerializeField] private Transform _playerTransform;

        private void Start()
        {
            _newPosition = transform.position;
            _newRotation = transform.rotation;
         
            _cameraTransform = _camera.transform;
            _newZoom = _cameraTransform.localPosition;
        }

        private void Update()
        {
            HandleMouseInput();
            HandleMovementInput();
        }

        private void HandleMouseInput()
        {
            // Camera Position
            if (Input.GetButtonDown("MovementDrag"))
            {
                _plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (_plane.Raycast(ray, out entry))
                {
                    _dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetButton("MovementDrag"))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (_plane.Raycast(ray, out entry))
                {
                    _drageCurrentPosition = ray.GetPoint(entry);

                    _newPosition = transform.position + _dragStartPosition - _drageCurrentPosition;
                }
            }

            // CameraRotation
            if (Input.GetButtonDown("RotationDrag"))
                _rotateStartPosition = Input.mousePosition;

            if (Input.GetButton("RotationDrag"))
                _rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;

            _rotateStartPosition = _rotateCurrentPosition; // Resetting to not loop

            _newRotation *= Quaternion.Euler(Vector3.up * (-difference.x * _mouseRotationAmount));

            // CameraZoom
            if (Input.GetAxis("Zoom") > 0)
                _newZoom += _zoomAmount;
            else if (Input.GetAxis("Zoom") < 0)
                _newZoom -= _zoomAmount;

            ApplyZoom();
        }

        private void HandleMovementInput()
        {
            // Center on player
            if (Input.GetButton("CenterCameraOnPlayer"))
                _newPosition = _playerTransform.position;

            // Fast or slow camera movement
            if (Input.GetButton("Fast"))
                _movementSpeed = _fastSpeed;
            else
                _movementSpeed = _normalSpeed;

            // Camera Position
            _newPosition += (transform.forward * _movementSpeed * Input.GetAxis("Vertical"));
            _newPosition += (transform.right * _movementSpeed * Input.GetAxis("Horizontal"));
            ApplyMovement();

            // Camera Rotation
            _newRotation *= Quaternion.Euler(Vector3.up * _keyRotationAmount * Input.GetAxis("Rotation"));
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, _movementTime * Time.deltaTime);
        }

        private void ApplyZoom()
        {
            // Setting limits
            if (_newZoom.y < _minZoom)
                _newZoom = new Vector3(0, _minZoom, -_minZoom);

            if (_newZoom.y > _maxZoom)
                _newZoom = new Vector3(0, _maxZoom, -_maxZoom);

            // Applying the zoom
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, _movementTime * Time.deltaTime);
        }

        private void ApplyMovement()
        {
            // SettingLimits
            if (_newPosition.x < _minPosition.x)
                _newPosition.x = _minPosition.x;

            if (_newPosition.z < _minPosition.z)
                _newPosition.z = _minPosition.z;

            if (_newPosition.x > _maxPosition.x)
                _newPosition.x = _maxPosition.x;

            if (_newPosition.z > _maxPosition.z)
                _newPosition.z = _maxPosition.z;

            // Appying the movement
            transform.position = Vector3.Lerp(transform.position, _newPosition, _movementTime * Time.deltaTime);
        }
    }
}