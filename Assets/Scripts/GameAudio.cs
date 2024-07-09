using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioSource asSounds;
    public AudioClip hitPlayerSound;
    public AudioClip winLevelSound;

    public void PlayHitPlayerSound()
    {
        asSounds.PlayOneShot(hitPlayerSound);
    }

    public void PlayWinLevelSound()
    {
        asSounds.PlayOneShot(winLevelSound);
    }
}
