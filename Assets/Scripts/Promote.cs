using UnityEngine;

public class Promote : MonoBehaviour
{
    public float cooldownDuration;
    private float _timeOfCooldown;

    public bool PromoteNodeAtPosition(Vector2 mousePos)
    {
        //Has the ability cooldown completed?
        if (Time.time < _timeOfCooldown + cooldownDuration)
        {
            //No! Warning message here?
            Debug.Log("Promote is on Cooldown! " + (_timeOfCooldown + cooldownDuration - Time.time) + " s remaining");
            return false;
        }

        //Get object at mouse point
        Node hitNode = Physics2D.OverlapPoint(mousePos)?.GetComponent<Node>();

        //Is this redundant because of ?. above?
        if (!hitNode) return false;

        //Node used successfully! Lets crush that misinformation!
        if (hitNode.type != NodeType.Misinformed)
        {
            hitNode.NodePromotion();
            StartCooldown();
            //Tool is consumed on cursor here
            return true;
        }

        return false;
    }

    public void StartCooldown()
    {
        _timeOfCooldown = Time.time;
        CooldownRadialManager.instance.promoteCooldownRadial.CooldownStarted(cooldownDuration);
    }
}