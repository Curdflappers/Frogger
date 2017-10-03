using UnityEngine;
using System.Collections;

public class DestroyOnExit : MonoBehaviour
{
    /// <summary>
    /// The behavior when something leaves this.
    /// Only triggered when the collider's gameobject has a rigidbody
    /// Why? I don't know...
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log(other.gameObject + " exited.");
        // other.gameObject.GetComponent<Rigidbody2D>().Sleep();
        Destroy(other.gameObject);
    }
}