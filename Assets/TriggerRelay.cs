using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerRelay : MonoBehaviour
{
    public UnityAction<Collider2D> enter;
    public UnityAction<Collider2D> exit;


    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private BoxCollider2D _collider;

    Vector2 prevSize = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        enter?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        exit?.Invoke(other);
    }

    private void Update()
    {
        if (prevSize != _rectTransform.rect.size)
        {
            prevSize = _rectTransform.rect.size;

            _collider.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(_rectTransform.position);
            _collider.size = ResizeCollider(_rectTransform);
        }

    }

    private Vector2 ResizeCollider(RectTransform rectTransform)
    {
        // Get the size of the RectTransform in pixels
        Vector2 canvasSize = rectTransform.sizeDelta;

        // Convert the RectTransform size from canvas space to screen space
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
        Vector3 worldSize = Camera.main.ScreenToWorldPoint(screenPoint + canvasSize) - Camera.main.ScreenToWorldPoint(screenPoint);

        // Set the BoxCollider size to match the RectTransform size in world space
        return new Vector2(worldSize.x, worldSize.y);
    }

}
