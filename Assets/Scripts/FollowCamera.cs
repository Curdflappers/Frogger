using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    void Start() { }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 pos = transform.position;
            float yDisplacement = target.transform.position.y + offset.y;
            transform.position = Vector3.Lerp(pos, new Vector3(pos.x, yDisplacement, pos.z), 0.1f);
        }
    }
}