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
    Cut,
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
        switch (newTool)
        {
            case "Promote":
                currentTool = CursorTools.Promote;
                break;
            
            case "Cut":
                currentTool = CursorTools.Cut;
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
                Click();
            }
        }
    }

    public void UseCurrentTool(Vector2 mouseClickPos)
    {
        bool wasToolUsedSuccessfully = false;
        
        switch (currentTool)
        {
            case CursorTools.Cut:
                wasToolUsedSuccessfully = unfollower.StartCut(mouseClickPos);
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

    public void Click()
    {
        //Get mouse position
        Vector2 mouseClickPos = Mouse.current.position.ReadValue();
                
        //Convert to world coords and set
        mouseClickPos = Camera.main.ScreenToWorldPoint(mouseClickPos);

        UseCurrentTool(mouseClickPos);

    }
}
