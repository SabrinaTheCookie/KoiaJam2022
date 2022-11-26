using UnityEngine;
using UnityEngine.UI;

public class CooldownRadial : MonoBehaviour
{

    public Image cooldownOverlayImage;
    public bool isCoolingDown;

    private float _cooldown;
    private float _cooldownEndTime;
    
    public void ButtonSelected()
    {
        cooldownOverlayImage.fillAmount = 1;
    }

    public void ButtonDeselected()
    {
        cooldownOverlayImage.fillAmount = 0;
    }

    public void CooldownStarted(float cooldownLength)
    {
        _cooldown = cooldownLength;
        _cooldownEndTime = Time.time + cooldownLength;
        isCoolingDown = true;
    }

    public void Update()
    {
        if (!isCoolingDown) return;

        if (Time.time < _cooldownEndTime)
        {
            //Cooldown still going
            UpdateRadial();
        }
        else
        {
            //Cooldown ended!
            isCoolingDown = false;
            ButtonDeselected();
        }
        
    }

    private void UpdateRadial()
    {
        //Get the cooldown remaining in 0-1 format. (remaining / cooldown)
        float cooldownRemaining = _cooldownEndTime - Time.time;
        
        cooldownRemaining = cooldownRemaining / _cooldown;
        cooldownOverlayImage.fillAmount = cooldownRemaining;
    }
}
