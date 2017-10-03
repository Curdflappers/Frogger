using UnityEngine;
using System.Collections;

public class CarBehavior : VehicleBehavior
{ 
    public override void Rotate()
    {
        // Debug.Log("Car rotating!...");
        if(LaneBehavior().LaneVelocity < 0) { transform.rotation = new Quaternion(0, 0, 1, 0); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag.Equals("Player")) { Destroy(obj); }
    }
}
