using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum CursorTools
{
    Promote,
    Unfollow,
    Select
}
public class PlayerCursor : MonoBehaviour
{
    
    public PlayerInput playerInput;
    public CursorTools currentTool = CursorTools.Select;

    public Promote promoter;
    public Unfollow unfollower;

    public void Awake()
    {
        
    }

    public void ChangeTool(CursorTools newTool)
    {
        currentTool = newTool;
    }
    
    public void ChangeTool(string newTool)
    {
        if (currentTool is CursorTools.Unfollow) unfollower.ClearCut();
        
        switch (newTool)
        {
            case "Promote":
                currentTool = CursorTools.Promote;
                break;
            
            case "Unfollow":
                currentTool = CursorTools.Unfollow;
                break;
            
            case "Select":
                currentTool = CursorTools.Select;
                break;
        }
    }

    public void Update()
    {
        //If a tool is selected
        if (currentTool is not CursorTools.Select)
        {
            //TODO Remove from every frame & Include touch screen support
            
            //If LMB is pressed
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                UseCurrentTool(GetMousePosition());
            }

            //If Unfollow - Also check for release or if its held
            if (currentTool is CursorTools.Unfollow)
            {
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    bool toolUsedSuccessfully = unfollower.EndCut(GetMousePosition());
                    if(toolUsedSuccessfully) ChangeTool(CursorTools.Select);
                }

                if (Mouse.current.leftButton.isPressed)
                {
                    unfollower.CheckMaxDist(GetMousePosition());
                }
            }
            
            
        }
    }

    public void UseCurrentTool(Vector2 mouseClickPos)
    {
        bool wasToolUsedSuccessfully = false;
        
        switch (currentTool)
        {
            case CursorTools.Unfollow:
                unfollower.StartCut(mouseClickPos);
                //Keep looking for mouse up
                break;
            case CursorTools.Promote:
                wasToolUsedSuccessfully = promoter.PromoteNodeAtPosition(mouseClickPos);
                break;
            case CursorTools.Select:
                break;
        }

        if (wasToolUsedSuccessfully)
        {
            ChangeTool(CursorTools.Select);
        }
    }

    public Vector2 GetMousePosition()
    {
        //Get mouse position
        Vector2 mouseClickPos = Mouse.current.position.ReadValue();
                
        //Convert to world coords and set
        mouseClickPos = Camera.main.ScreenToWorldPoint(mouseClickPos);

        return mouseClickPos;
    }
}
