using UnityEngine;
using System.Collections;

public class RiverBehavior : LaneBehavior
{
    new void Start()
    {
        base.Start();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DestroyIfDrowning(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        DestroyIfDrowning(other.gameObject);
    }

    void DestroyIfDrowning(GameObject go)
    {
        if (!IsSafe(go) && go.tag.Equals("Player"))
        {
            Destroy(go);
        }
    }

    /// <summary>
    /// Returns whether go's collider is overlapping any of this lane's transform's children's transforms
    /// </summary>
    /// <param name="go"></param>
    /// <returns>True if overlap</returns>
    bool IsSafe(GameObject go)
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            // if this child is overlapping the parameter
            if(transform.GetChild(i).GetComponent<BoxCollider2D>().IsTouching(go.GetComponent<BoxCollider2D>()))
            {
                return true;
            }
        }
        return false;
    }
}
