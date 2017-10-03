using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    /// <summary> The lanes in this world </summary>
    private List<LaneBehavior> _lanes;

    /// <summary> The lanes generated within this world </summary>
    public List<LaneBehavior> Lanes { get { return _lanes; } }

    /// <summary> The frogs in this world </summary>
    private List<GameObject> _frogs;

    /// <summary> The frogs hopping around in this world </summary>
    public List<GameObject> Frogs { get { return _frogs; } }

    /// <summary> The possible lane types to generate </summary>
    private GameObject[] _laneTypes;

    /// <summary> The possible lane types to generate </summary>
    public GameObject[] LaneTypes { get { return _laneTypes; } }

    /// <summary> All possible lane types for all worlds </summary>
    private static GameObject[] _allLaneTypes;

    /// <summary> All possible lane types </summary>
    public GameObject[] AllLaneTypes { get { return _allLaneTypes; } }

    public int YMax, YMin;
    public bool MustSpawnNewLane;

    void Start()
    {
        if(_allLaneTypes == null)
        {
            _allLaneTypes = Resources.LoadAll<GameObject>("Prefabs/Lanes");
        }
        _laneTypes = _allLaneTypes; // this world can load any lane type
        _lanes = new List<LaneBehavior>(); // no lanes yet
        _frogs = new List<GameObject>(); // no frogs yet
        AddFrog(GameObject.Find("Frog")); // add the first frog
        LoadInitialLanes(); // load the starter lanes
        YMax = 0;
        YMin = 0;
    }

    /// <summary>
    /// Make all visible lanes grass lanes
    /// </summary>
    void LoadInitialLanes()
    {
        int lanesVisible = LanesVisible();
        for(int i = 0; i < lanesVisible; i++)
        {
            GameObject newLane = Resources.Load<GameObject>("Prefabs/Lanes/Grass");
            AddLane(newLane, true);
        }
    }

    /// <summary>
    /// Create a lane of the correct type at specified position, then add this to our list of lanes
    /// </summary>
    /// <param name="forward"></param>
    void SpawnLane(bool forward)
    {
        GameObject newLane = NewLaneType();
        AddLane(newLane, forward);
    }

    /// <summary>
    /// Returns the lane type to be spawned.
    /// </summary>
    /// <returns>A lane type based on difficulty of game</returns>
    GameObject NewLaneType()
    {
        // return _laneTypes[Random.Range(0, _laneTypes.Length)]; // old way; completely random
        float random = Random.Range(0f, 1);
        float[] laneProbs = LaneProbabilities();
        double max = 0;
        for (int i = 0; i < laneProbs.Length; i++)
        {
            max += laneProbs[i];
            if(random < max)
            {
                if(i == 0) { return (GameObject)Resources.Load("Prefabs/Lanes/Grass"); }
                if(i == 1) { return (GameObject)Resources.Load("Prefabs/Lanes/Road"); }
                return (GameObject)Resources.Load("Prefabs/Lanes/Log River");
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the chance of spawning each lane based on the difficulty saturation
    /// </summary>
    /// <returns>A list of floats, each in (0, 1) 
    /// for each grass, road, log (in that order) </returns>
    float[] LaneProbabilities()
    {
        float DS = DifficultySaturation();
        return new float[] { 1f/3 - DS/3, 1f/3 + DS/3, 1f/3 + DS/3 };
    }

    /// <summary>
    /// How difficult the game is on a scale of 0 to 1, where 0 is easy and 1 is max difficulty
    /// </summary>
    /// <returns></returns>
    float DifficultySaturation()
    {
        int count = _lanes.Count;
        // logistic function 1/(1+e^-0.05(x-50))
        return 1.0f / (1 + Mathf.Exp(-0.05f * (count - 50)));
    }

    /// <summary>
    /// Loads the lane at the necessary position
    /// </summary>
    /// <param name="forward"></param>
    public void LoadLane(bool forward)
    {
        if(MustSpawnNewLane) { SpawnLane(forward); }
        else
        {
            Instantiate(_lanes[SpawnIndex(forward)], new Vector3(0, SpawnPos(forward)), new Quaternion());
        }
    }

    /// <summary>
    /// Returns the index of the lane that needs to be spawned
    /// </summary>
    /// <param name="forward"></param>
    /// <returns></returns>
    int SpawnIndex(bool forward)
    {
        FrogBehavior frogBehavior = _frogs[0].GetComponent<FrogBehavior>();
        if(forward) { return frogBehavior.YRelative - YMin + LanesVisible(); }
        return frogBehavior.YRelative - YMin - 1;
    }

    /// <summary>
    /// The spawn position of a new lane, either forward or behind
    /// </summary>
    /// <param name="forward"></param>
    /// <returns></returns>
    int SpawnPos(bool forward)
    {
        GameObject cam = GameObject.Find("Main Camera");
        float center = cam.GetComponent<FollowCamera>().offset.y;
        float fullHeight = cam.GetComponent<BoxCollider2D>().size.y;
        if (forward) { return Mathf.RoundToInt(center + fullHeight / 2 + 1); }
        return Mathf.RoundToInt(center - fullHeight / 2 - 1);
    }

    /// <summary>
    /// Return the number of lanes that can be viewed at once 
    /// including partial lanes
    /// </summary>
    /// <returns></returns>
    int LanesVisible()
    {
        return LanesInFrontOf() + LanesBehind() + 1;
    }

    /// <summary>
    /// Number of loaded lanes in front of the frog (pos.y greater than 0)
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="boundsHalfHeight"></param>
    /// <returns></returns>
    int LanesInFrontOf()
    {
        GameObject cam = GameObject.Find("Main Camera");
        float offset = cam.GetComponent<FollowCamera>().offset.y;
        float boundsHalfHeight = cam.GetComponent<BoxCollider2D>().size.y / 2;
        return (int)(offset + boundsHalfHeight);
    }

    /// <summary>
    /// Number of loaded lanes behind the frog (pos.y less than 0)
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="boundsHalfHeight"></param>
    /// <returns></returns>
    int LanesBehind()
    {
        GameObject cam = GameObject.Find("Main Camera");
        float offset = cam.GetComponent<FollowCamera>().offset.y;
        float boundsHalfHeight = cam.GetComponent<BoxCollider2D>().size.y * cam.transform.lossyScale.y / 2;
        return Mathf.Abs((int)(offset - boundsHalfHeight));
    }

    /// <summary>
    /// Add and keep track of the specified lane, inserting it into the list depending on forward or backward
    /// </summary>
    /// <param name="lane"></param>
    /// <param name="forward"></param>
    void AddLane(GameObject lane, bool forward)
    {
        Instantiate(lane, new Vector3(0, SpawnPos(forward)), new Quaternion());
        if (forward) { _lanes.Add(lane.GetComponent<LaneBehavior>()); }
        else { _lanes.Insert(0, lane.GetComponent<LaneBehavior>()); }
    }

    /// <summary>
    /// Track a new frog, inform frog of its world
    /// </summary>
    /// <param name="frog"></param>
    void AddFrog(GameObject frog)
    {
        _frogs.Add(frog);
        frog.GetComponent<FrogBehavior>().World = this;
    }
}
