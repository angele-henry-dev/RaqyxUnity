using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Animator animator;

    public void Highlight()
    {
        animator.SetTrigger("highlight");
    }

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}
