using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the frog's last score on the text component of the gameobject 
/// this is attached to.
/// Does not update
/// </summary>
public class LastScoreDisplay : MonoBehaviour
{
    void Start ()
    {
        GetComponent<Text>().text = "" + FrogBehavior.LastScore;
	}
}
