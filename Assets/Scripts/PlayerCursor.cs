using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CursorTools
{
    Promote,
    Cut,
    None
}
public class PlayerCursor : MonoBehaviour
{
    public CursorTools currentTool = CursorTools.None;

    public void ChangeTool(CursorTools newTool)
    {
        currentTool = newTool;
    }

    public void Update()
    {
        //Get input for mouseclick or tap
    }
}
