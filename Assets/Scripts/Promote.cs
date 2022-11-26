using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Promote : MonoBehaviour
{
    public float cooldownDuration;
    public float timeOfCooldown;
    
    public bool PromoteNodeAtPosition(Vector2 mousePos)
    {
        //Has the ability cooldown completed?
        if (Time.time < timeOfCooldown + cooldownDuration)
        {
            //No! Warning message here?
            Debug.Log("Promote is on Cooldown! " + (timeOfCooldown + cooldownDuration - Time.time) + " s remaining");
            return false;
        }
            
        //Get object at mouse point
        Node hitNode = Physics2D.OverlapPoint(mousePos)?.GetComponent<Node>();
        
        //Is this redundant because of ?. above?
        if (!hitNode) return false;
        
        //Node used successfully! Lets crush that misinformation!
        hitNode.NodePromotion();

        timeOfCooldown = Time.time;

        return true;
    }
}
