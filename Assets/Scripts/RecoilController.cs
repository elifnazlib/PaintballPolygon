using UnityEngine;

public class RecoilController : MonoBehaviour
{
    private Animator paintballGunAnimator; // Animator for the paintball gun to show recoil effect

    private void Awake()
    {
        paintballGunAnimator = GetComponent<Animator>(); // Getting the Animator component from the GameObject
    }

    public void SetRecoilBoolToFalse()
    {
        paintballGunAnimator.SetBool("shoot", false); // Setting the recoil animation to false
    }
}
