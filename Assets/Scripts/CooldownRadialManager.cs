using UnityEngine;

public class CooldownRadialManager : MonoBehaviour
{
    public static CooldownRadialManager instance;

    public CooldownRadial promoteCooldownRadial; 
    public CooldownRadial unfollowCooldownRadial; 
    public CooldownRadial verifyCooldownRadial;

    public void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        GameController.StopGame += ResetRadials;
    }

    private void ResetRadials()
    {
        promoteCooldownRadial.ResetRadial();
        unfollowCooldownRadial.ResetRadial();
        verifyCooldownRadial.ResetRadial();
    }

    private void OnDisable()
    {
        GameController.StopGame -= ResetRadials;
    }

    public CooldownRadial GetRadial(int button)
    {
        CooldownRadial result = button switch
        {
            0 => promoteCooldownRadial,
            1 => unfollowCooldownRadial,
            2 => verifyCooldownRadial,
            _ => promoteCooldownRadial
        };

        return result;
    }
}
