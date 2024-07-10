using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private AudioSource asSounds;
    [SerializeField]
    private AudioClip hitPlayerSound;
    [SerializeField]
    private AudioClip winLevelSound;

    public void PlayHitPlayerSound()
    {
        asSounds.PlayOneShot(hitPlayerSound);
    }

    public void PlayWinLevelSound()
    {
        asSounds.PlayOneShot(winLevelSound);
    }
}
