using UnityEngine;
using System.Collections;

public class LilypadBehavior : VehicleBehavior
{
    /// <summary>
    /// Do nothing. Lilypads don't move!
    /// </summary>
    public override void SetVelocity() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            other.transform.position = transform.position;
        }
    }
}
