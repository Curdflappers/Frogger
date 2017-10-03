using UnityEngine;
using System.Collections;

public class VehicleBehavior : MonoBehaviour
{
    public LaneBehavior LaneBehavior() { return gameObject.GetComponentInParent<LaneBehavior>(); }

    public virtual void Rotate() { }
    public virtual void SetVelocity()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(LaneBehavior().LaneVelocity, 0));
    }

    void OnDestroy()
    {
        if (LaneBehavior())
        {
            LaneBehavior().RespawnVehicle(gameObject);
        }
    }
}
