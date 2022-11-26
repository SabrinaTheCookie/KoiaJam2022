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
        CooldownRadial result = button switch
        {
            0 => promoteCooldownRadial,
            1 => unfollowCooldownRadial,
            2 => selectCooldownRadial,
            _ => promoteCooldownRadial
        };

        return result;
    }
}
