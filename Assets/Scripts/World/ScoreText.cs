using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private Animator animator;

    private const string triggerHightlight = "highlight";

    public void Highlight()
    {
        animator.SetTrigger(triggerHightlight);
    }

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}
