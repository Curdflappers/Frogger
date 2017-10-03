using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A LaneBehavior is a lane that contains one type of vehicle
/// </summary>
public class LaneBehavior : MonoBehaviour
{
    public GameObject vehicle;
    private float laneVelocity;
    protected Camera mainCamera;
    private float minSpeed, maxSpeed;
    private int minVehicles, maxVehicles;

    public float LaneVelocity { get { return laneVelocity; } }
    public float MinSpeed
    {
        get { return minSpeed; }
        set
        {
            if (value < 0) minSpeed = 0;
            else if (value > maxSpeed)
            {
                maxSpeed = value;
                minSpeed = value;
            }
        }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set
        {
            if(value < MinSpeed)
            {
                MinSpeed = value; // still will not go lower than 0
            }
            if (value < 0)
            {
                maxSpeed = 0;
            }
        }
    }

    /// <summary>
    /// All the vehicles currently in this lane
    /// </summary>
    public List<GameObject> Vehicles
    {
        get
        {
            List<GameObject> list = new List<GameObject>();
            for(int i = 0; i < VehicleCount; i++)
            {
                list.Add(transform.GetChild(i).gameObject);
            }
            return list;
        }
    }

    public int VehicleCount { get { return transform.childCount; } }

    protected void Start()
    {
        GameObject cameraRig = GameObject.Find("Main Camera");
        mainCamera = cameraRig.GetComponent<Camera>();

        transform.localScale = new Vector3(cameraRig.GetComponent<BoxCollider2D>().size.x, 1, 1);
        GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        
        SetLaneVelocity();
        SpawnVehicles();
    }

    /// <summary>
    /// Instantiates the vehicles of this lane
    /// </summary>
    void SpawnVehicles()
    {
        int vehicleCount = (int)(Random.value * 4 + 1);
        List<float> remainingPositions = AllPositions();
        for (int i = 0; i < vehicleCount; i++)
        {
            int index = (int)(Random.value * remainingPositions.Count); // choose a random position
            float pos = remainingPositions[index];
            remainingPositions.RemoveAt(index);
            SpawnVehicle(pos);
        }
    }

    /// <summary>
    /// Returns the width required to fit an entire vehicle
    /// </summary>
    /// <returns></returns>
    float SlotLength()
    {
        return vehicle.GetComponent<BoxCollider2D>().size.x*vehicle.transform.lossyScale.x;
    }

    List<float> AllPositions()
    {
        List<float> positions = new List<float>();
        float slotLength = SlotLength();
        double maxPosition = GetCameraWidth() / slotLength;
        for(int i = 0; i < maxPosition; i++)
        {
            positions.Add(i * slotLength - (float)GetCameraWidth() / 2);
        }
        return positions;
    }

    void SpawnVehicle(float xPos)
    {
        GameObject newVehicle = (GameObject)Instantiate(vehicle, new Vector3(xPos, transform.position.y), new Quaternion());
        VehicleBehavior vehicleBehavior = newVehicle.GetComponent<VehicleBehavior>();
        newVehicle.transform.parent = gameObject.transform; // Assign vehicle its lane
        vehicleBehavior.Rotate();
        vehicleBehavior.SetVelocity(); // set forth vehicle
    }

     /// <summary>
     /// Returns the width of this camera. Full distance from left side to right side
     /// </summary>
     /// <returns></returns>
    public double GetCameraWidth()
    {
        if (mainCamera) { return mainCamera.orthographicSize * 32 / 9; }
        return 0;
    }

    public void RespawnVehicle(GameObject myVehicle)
    {
        float distanceFromCenter = (float)GetCameraWidth()/2 + vehicle.GetComponent<BoxCollider2D>().size.x / 2 + 1;
        if (laneVelocity < 0) { SpawnVehicle(distanceFromCenter); }
        else { SpawnVehicle(-distanceFromCenter); }
    }

    void SetLaneVelocity()
    {
        laneVelocity = Random.Range(minSpeed, maxSpeed);
        if (Random.value < 0.5) { laneVelocity *= -1; } // 50% chance of flipping
    }
}