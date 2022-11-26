using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Unfollow : MonoBehaviour
{
    public Vector2 cutStartPos;
    public Vector2 cutEndPos;
    public float minCutDistance;
    
    public bool cutStarted;

    public LayerMask linkLayerMask;

    public void StartCut(Vector2 newCutStartPos)
    {
        cutStarted = true;
        cutStartPos = newCutStartPos;
        
        Debug.Log("Unfollow Cut Started");
    }

    //Called when the mouse is released after a hold while Unfollow is selected
    public bool EndCut(Vector2 newCutEndPos)
    {
        
        if (!cutStarted) return false; //Cut hasn't been started, why is one being ended?
        cutEndPos = newCutEndPos;
        
        Debug.Log("Unfollow Cut Ended");
        //If cursor position is closer than minCutDistance on cursor deselect, deselect the cut
        float distance = Vector2.Distance(cutStartPos, cutEndPos);
        if (distance < minCutDistance)
        {
            Debug.Log("Distance was too short! Unfollow cut failed");
            return false;
        }

        //Valid line, the misinformation has been sealed forever... haha jk... unless..?
        Debug.DrawLine(cutStartPos, cutEndPos, Color.red, Mathf.Infinity);
        
        //Links are on Z:5
        Vector3 offset = new Vector3(0, 0, 5);
        //Check for link collisions
        RaycastHit2D[] hits = Physics2D.RaycastAll(cutStartPos, cutEndPos - cutStartPos, distance, linkLayerMask, 0, 6);

        Debug.Log(hits.Length);
        //for each link hit
        foreach (RaycastHit2D hit in hits)
        {
            Link link = hit.transform.GetComponent<Link>();

            if (link)
            {
                link.LinkUnfollowed();
            }
        }
        
        ClearCut();

        return true;
    }

    public void ClearCut()
    {
        if (!cutStarted) return; //Stops double clears
        
        cutStarted = false;
        cutStartPos = Vector3.zero;
        cutEndPos = Vector3.zero;
    }
    
    
}
