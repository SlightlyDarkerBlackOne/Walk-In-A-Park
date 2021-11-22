using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scent : MonoBehaviour{
    [SerializeField]
    private GameObject scentParticle;
    private GameObject scent;
    public static bool ClickedScentIcon = false;
    private GameObject playerHuman;
    private bool scenting;
    public LayerMask detectLayer;
    public static bool ballFound = false;
    private Collider2D ball;
    public GameObject toDoListPanel;
    private bool checkedVladoScent = false;

    void Start() {
        playerHuman = GameObject.FindWithTag("Human");
        detectLayer = LayerMask.GetMask("Item");
        toDoListPanel = GameObject.Find("ToDo List Panel");
    }

    // Update is called once per frame
    void Update() {
        if (!scenting && (Input.GetKeyDown(KeyCode.G) || ClickedScentIcon)) {
            checkedVladoScent = true;
            StartCoroutine("ScentCloud");
            if (ClickedScentIcon) ClickedScentIcon = false;
        }
        if (scent) scent.transform.position = playerHuman.transform.position;

        ball = Physics2D.OverlapCircle(transform.position, 5f, detectLayer);
        if (ball != null && ball.name == "Vlado Ball" && checkedVladoScent) {
            ballFound = true;
            toDoListPanel.GetComponent<TaskManager>().CheckTaskOnToDoList();
            if (ballFound) Debug.Log("Vlado's ball found");
            if (!scenting) StartCoroutine("ScentCloud");
            if (scent) scent.transform.position = ball.transform.position;
        }
    }

    private IEnumerator ScentCloud() {
        scenting = true;
        scent = Instantiate(scentParticle, Vector2.zero, Quaternion.Euler(0, 0, -118f)) as GameObject;
        if (!ballFound) scent.transform.position = playerHuman.transform.position;
        else if (ball != null) scent.transform.position = ball.transform.position;
        yield return new WaitForSeconds(3);
        Destroy(scent);
        scenting = false;
    }
}