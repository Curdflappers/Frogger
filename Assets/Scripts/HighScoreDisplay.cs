using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the high score on the text component of this game object
/// </summary>
public class HighScoreDisplay : MonoBehaviour
{
	void Start ()
    {
        GetComponent<Text>().text = FrogBehavior.HighScore.ToString();
	}

    void Update()
    {
        GetComponent<Text>().text = FrogBehavior.HighScore.ToString();
    }
}
