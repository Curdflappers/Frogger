using UnityEngine;
using System.Collections;

public class LogBehavior : VehicleBehavior
{
    /// <summary>
    /// If collider belongs to player, 
    /// set the velocity of the player to the velocity of this
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            SetVelocity(other.gameObject, transform.parent.GetComponent<LaneBehavior>().LaneVelocity);
        }
    }

    /// <summary>
    /// If player, reset velocity of object to 0
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            SetVelocity(other.gameObject, 0);
        }
    }

    /// <summary>
    /// Set the velocity of go to velocity in the x-direction
    /// </summary>
    /// <param name="go"></param>
    /// <param name="velocity"></param>
    void SetVelocity(GameObject go, float velocity)
    {
        if (go.GetComponent<Rigidbody2D>())
        {
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0);
        }
    }
}
