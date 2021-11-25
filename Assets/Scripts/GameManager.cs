using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool leashActive;
    private List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] private Transform[] waypointListTransform;
    #region Singleton
    public static GameManager Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start() {
        FillWaypointList();
    }
    public Vector3 RandomWaypoint() {
        int randomIndex = Random.Range(0, waypoints.Count);
        return waypoints[randomIndex];
    }
    private void FillWaypointList() {
        foreach (Transform waypoint in waypointListTransform) {
            waypoints.Add(waypoint.position);
        }
    }
}
