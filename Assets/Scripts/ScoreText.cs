using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}
