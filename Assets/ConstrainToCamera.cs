using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private float cameraOrthographicSize;
    private float cameraAspectRatio;
    private Rigidbody2D _rigidBody;

    void Start()
    {
        _mainCamera = Camera.main;
        _rigidBody= GetComponent<Rigidbody2D>();
        if (_mainCamera != null)
        {
            cameraOrthographicSize = _mainCamera.orthographicSize;
            cameraAspectRatio = _mainCamera.aspect;
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void Update()
    {
        if (_rigidBody != null && _mainCamera != null)
        {
            // Calculate the camera boundaries
            float cameraWidth = cameraOrthographicSize * cameraAspectRatio;
            float cameraHeight = cameraOrthographicSize;

            // Constrain object's position within camera boundaries
            float clampedX = Mathf.Clamp(_rigidBody.position.x, -cameraWidth, cameraWidth);
            float clampedY = Mathf.Clamp(_rigidBody.position.y, -cameraHeight, cameraHeight);

            // Set the constrained position
            _rigidBody.position = new Vector2(clampedX, clampedY);
        }
    }
}
