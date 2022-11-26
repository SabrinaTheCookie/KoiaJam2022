using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CooldownRadialManager : MonoBehaviour
{
    public static CooldownRadialManager instance;

    public CooldownRadial promoteCooldownRadial; 
    public CooldownRadial unfollowCooldownRadial; 
    public CooldownRadial selectCooldownRadial; 
    
    public void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public CooldownRadial GetRadial(int button)
    {
        CooldownRadial result;
        
        switch (button)
        {
            case 0:
                result =  promoteCooldownRadial;
                break;
            case 1:
                result = unfollowCooldownRadial;
                break;
            case 2:
                result = selectCooldownRadial;
                break;
            default:
                result = promoteCooldownRadial; //Default to promote. Shouldn't happen.
                break;
        }
        

        return result;
    }
}
