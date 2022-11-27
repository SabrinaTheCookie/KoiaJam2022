using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;

public class UnfollowVFX : MonoBehaviour
{
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    public float vfxFadeDelay = 1.5f;
    public float minSpriteSize = 0.5f;

    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    private bool _shouldUpdate;
    
    [SerializeField] private InputAction pos;
    
    private void OnEnable()
    {
        pos.Enable();
    }

    private void OnDisable()
    {
        pos.Disable();
    }

    private void Start()
    {
        EndFX(false);
    }

    public void ShowFX()
    {
        _shouldUpdate = true;
        spriteRenderer.enabled = true;
        _startPosition = PlayerCursor.GetMousePosition(pos.ReadValue<Vector2>());
        _currentPosition = _startPosition;

        UpdateTransform();
    }

    public void EndFX(bool success)
    {
        _shouldUpdate = false;
        if (!success) spriteRenderer.enabled = false;
        else
        {
            //wait duration, then remove.
            StartCoroutine(SpriteRendererEnableDelay(false, vfxFadeDelay));
        }
    }

    private void UpdateTransform()
    {
        //Move to center of lines
        Vector2 center = (_startPosition + _currentPosition) / 2;
        transform.position = center;

        //Set widthX to line distance.
        float widthX = Vector2.Distance(_startPosition, _currentPosition);
        if (widthX < minSpriteSize) widthX = minSpriteSize;
        Vector2 newSpriteSize = new Vector2(widthX, minSpriteSize);
        spriteRenderer.size = newSpriteSize;
        
        
        //TODO Fix to get cut working
        //Set sprite rotation y2-y1, x2-x1
        float angle = Mathf.Atan2(_currentPosition.y - _startPosition.y, _currentPosition.x - _startPosition.x) * Mathf.Rad2Deg;

        Quaternion newRot = Quaternion.Euler(0, 0, angle);
        
        //Rotate
        transform.rotation = newRot;

    }

    private IEnumerator SpriteRendererEnableDelay(bool newEnabled, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        spriteRenderer.enabled = newEnabled;
    }

    private void Update()
    {
        if (!_shouldUpdate) return;
        
        _currentPosition = PlayerCursor.GetMousePosition(pos.ReadValue<Vector2>());
        UpdateTransform();
    }
}
