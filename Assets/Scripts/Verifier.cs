using System;
using UnityEngine;

public class Verifier : MonoBehaviour
{
    public static event Action NodeVerified;
    public float cooldownDuration;
    private float _timeOfCooldown;
    
    private void Awake()
    {
        _timeOfCooldown = Time.time - cooldownDuration;
    }
    
    public bool VerifyNodeAtPos(Vector2 mousePos)
    {
        //Has the ability cooldown completed?
        if (Time.time < _timeOfCooldown + cooldownDuration)
        {
            //No! Warning message here?
            Debug.Log("Verify is on Cooldown! " + (_timeOfCooldown + cooldownDuration - Time.time) + "s remaining");
            return false;
        }

        //Get object at mouse point
        Node hitNode = Physics2D.OverlapPoint(mousePos)?.GetComponent<Node>();

        //Is this redundant because of ?. above?
        if (!hitNode) return false;

        //Node used successfully! Lets crush that misinformation!
        if (hitNode.CanBeVerified())
        {
            hitNode.Source.VerifySource();
            NodeVerified?.Invoke();
            StartCooldown();
            //Tool is consumed on cursor here
            return true;
        }

        return false;
    }

    private void StartCooldown()
    {
        _timeOfCooldown = Time.time;
        CooldownRadialManager.instance.verifyCooldownRadial.CooldownStarted(cooldownDuration);
    }
    private void OnEnable()
    {
        GameController.StopGame += GameReset;
    }

    private void GameReset(string s)
    {
        _timeOfCooldown = Time.time - cooldownDuration;
    }

    private void OnDisable()
    {
        GameController.StopGame -= GameReset;
    }
    
}
