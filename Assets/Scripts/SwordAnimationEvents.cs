using UnityEngine;

public class SwordAnimationEvents : MonoBehaviour
{
    public FirstPersonController player;

    public void PerformAttackHit()
    {
        player.PerformAttackHit();
    }

    public void OnAttackFinished()
    {
        player.OnAttackFinished();
    }
}

