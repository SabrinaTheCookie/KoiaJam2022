using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class UnfollowVFX : MonoBehaviour
{
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    public float vfxFadeDelay = 1.5f;
    public float minSpriteSize = 0.5f;

    private Vector2 startPosition;
    private Vector2 currentPosition;
    private bool shouldUpdate;

    private void Start()
    {
        EndFX(false);
    }

    public void ShowFX()
    {
        shouldUpdate = true;
        spriteRenderer.enabled = true;
        startPosition = PlayerCursor.GetMousePosition();
        currentPosition = startPosition;

        UpdateTransform();
    }

    public void EndFX(bool success)
    {
        shouldUpdate = false;
        if (!success) spriteRenderer.enabled = false;
        else
        {
            //wait duration, then remove.
            StartCoroutine(SpriteRendererEnableDelay(false, vfxFadeDelay));
        }
    }

    public void UpdateTransform()
    {
        //Move to center of lines
        Vector2 center = (startPosition + currentPosition) / 2;
        transform.position = center;

        //Set widthX to line distance.
        float widthX = Vector2.Distance(startPosition, currentPosition);
        if (widthX < minSpriteSize) widthX = minSpriteSize;
        Vector2 newSpriteSize = new Vector2(widthX, minSpriteSize);
        spriteRenderer.size = newSpriteSize;
        
        
        //TODO Fix to get cut working
        //Set sprite rotation y2-y1, x2-x1
        float angle = Mathf.Atan2(currentPosition.y - startPosition.y, currentPosition.x - startPosition.x) * Mathf.Rad2Deg;

        Quaternion newRot = Quaternion.Euler(0, 0, angle);
        
        //Rotate
        transform.rotation = newRot;

    }

    public IEnumerator SpriteRendererEnableDelay(bool newEnabled, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        spriteRenderer.enabled = newEnabled;
    }

    private void Update()
    {
        if (shouldUpdate)
        {
            currentPosition = PlayerCursor.GetMousePosition();

            UpdateTransform();
        }
    }
}
