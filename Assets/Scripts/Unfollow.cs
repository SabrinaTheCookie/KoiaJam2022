using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Unfollow : MonoBehaviour
{
    public Vector2 cutStartPos;
    public Vector2 minCutDistance;
    
    public bool cutStarted;
    
    public bool StartCut(Vector2 newCutStartPos)
    {
        cutStarted = true;
        cutStartPos = newCutStartPos;
        
        Debug.Log("Cut Started");

        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (cutStarted)
        {
            //If cursor position is closer than minCutDistance on cursor deselect, deselect the cut
        }
    }
    
    
}
