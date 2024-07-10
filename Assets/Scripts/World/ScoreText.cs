using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private Animator animator;

    public void Highlight()
    {
        animator.SetTrigger("highlight");
    }

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}
