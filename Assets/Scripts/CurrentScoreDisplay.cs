using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the current score of the player frog in the text component 
/// in the object this is attached to. 
/// Assumes exactly one player frog in the scene.
/// </summary>
public class CurrentScoreDisplay : MonoBehaviour
{
    FrogBehavior frog;

    void Start ()
    {
        frog = GameObject.FindWithTag("Player").GetComponent<FrogBehavior>();
	}
	
	void Update ()
    {
        GetComponent<Text>().text = "" + frog.Score;
	}
}
