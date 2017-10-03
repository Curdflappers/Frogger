using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrogBehavior : MonoBehaviour
{
    int score = 0;
    public static int highScore = 0;
    static int lastScore = 0;
    int _yRelative = 0;
    /// <summary>
    /// The distance between the frog and the furthest lane generated
    /// </summary>
    public int YRelative { get { return _yRelative; } }
    GameObject cam;
    public World World;
    public GameObject lane;

    public int Score { get { return score; } }
    public static int LastScore { get { return lastScore; } }
    public static int HighScore { get { return highScore; } }

    /// <summary>
    /// Adjusts camera hitbox bounds, resets score texts
    /// </summary>
    void Start()
    {
        SaveLoad.Load();
        cam = GameObject.Find("Main Camera");
        float fullHeight = 2 * (cam.GetComponent<Camera>().orthographicSize);
        cam.GetComponent<BoxCollider2D>().size =
            new Vector2(16f / 9 * fullHeight, fullHeight);
    }

    /// <summary>
    /// Helper, returns cam.GetComponent<Camera>().orthographicsSize;
    /// </summary>
    /// <returns>cam.GetComponent<Camera>().orthographicsSize</returns>
    double CameraSize() { return cam.GetComponent<Camera>().orthographicSize; }

    /// <summary>
    /// Reads for movement inputs and moves in response
    /// </summary>
    void Update()
    {
        if (ShouldMoveForward()) { MoveForward(); }
        if (ShouldMoveBackward()) { MoveBackward(); }
        if (ShouldMoveLeft()) { MoveLeft(); }
        if (ShouldMoveRight()) { MoveRight(); }
    }

    private bool ShouldMoveForward()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }

    private bool ShouldMoveBackward()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
    }

    private bool ShouldMoveLeft()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
    }

    private bool ShouldMoveRight()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
    }

    /// <summary>
    /// Loads the next lane, rotates the frog, moves all else down, and updates World.MaxForward
    /// </summary>
    public void MoveForward()
    {
        World.MustSpawnNewLane = (_yRelative == World.YMax);
        World.LoadLane(true);
        transform.rotation = Quaternion.identity;
        MoveAll(Vector3.down);
        if (_yRelative > World.YMax)
        {
            World.YMax = _yRelative;
        }
        UpdateScore();
    }

    /// <summary>
    /// Loads the next lane, rotates the frog, moves all else up, and updates World.MaxBackward
    /// </summary>
    public void MoveBackward()
    {
        World.MustSpawnNewLane = (_yRelative == World.YMin);
        World.LoadLane(false);
        transform.rotation = new Quaternion(0, 0, 1, 0);
        MoveAll(Vector3.up);
        if (_yRelative < World.YMin)
        {
            World.YMin = _yRelative;
            World.MustSpawnNewLane = true;
        }
    }

    public void MoveLeft()
    {
        transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f); // face left
        transform.Translate(Vector3.left, Space.World);

    }

    public void MoveRight()
    {
        transform.rotation = new Quaternion(0, 0, 0.7f, -0.7f); // face right
        transform.Translate(Vector3.right, Space.World);
    }

    /// <summary>
    /// Update the appropriate score displays
    /// </summary>
    void UpdateScore()
    {
        if (_yRelative > score)
        {
            score++;
        }
        if (score > highScore)
        {
            highScore++;
        }
    }

    /// <summary>
    /// Move all parentless, non-player, or non-UI elements.
    /// </summary>
    /// <param name="translation"></param>
    void MoveAll(Vector3 translation)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            // Move all parentless, non-player, and non-UI elements
            if (!(go.transform.parent || go.tag.Equals("Player") || go.layer == 5))
            { go.transform.Translate(translation, Space.World); }
        }
        _yRelative -= (int)translation.y;
    }

    /// <summary>
    /// Reloads the level, saving the last score
    /// </summary>
    void OnDestroy()
    {
        SaveLoad.Save();
        lastScore = score;
        SceneManager.LoadScene("MainScene");
    }
}
