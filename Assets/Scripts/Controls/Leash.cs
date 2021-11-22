using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leash : MonoBehaviour
{
    private GameObject leash;
    public GameObject playerHuman;
    public GameObject boneToJoint;
    private HingeJoint2D joint;
    [SerializeField]
    private float minLeashDistance = 10f;
    public static bool puttingOnLeash = true;
    private Vector2 previousPosition;
    private LayerMask detectLayer;
    private AgentEnemy agentEnemyScript;
    private MoveWaypoints moveWaypoints;

    // Start is called before the first frame update
    void Start()
    {
        leash = transform.Find("Leash").gameObject;
        joint = playerHuman.GetComponent<HingeJoint2D>();

        detectLayer = LayerMask.GetMask("Human");
        agentEnemyScript = playerHuman.GetComponent<AgentEnemy>();
        moveWaypoints = playerHuman.GetComponent<MoveWaypoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || ClickedOnVlado() == true) 
        {
            if (!leash.activeSelf && DogInRange())
            {
                puttingOnLeash = true;
                SFXManager.Instance.PlaySound(SFXManager.Instance.leashAttach);
            }
            else if (leash.activeSelf)
            {
                DisconnectLeashFromPlayerHuman();
            }
        }
        
        //necessary to separate this logic because of Lerp (needs to be in Update, not in if block)
        if (puttingOnLeash)
        {
            playerHuman.transform.position = Vector2.Lerp(playerHuman.transform.position,
                boneToJoint.transform.position, 5f*Time.deltaTime);

            if (Vector2.Distance(playerHuman.transform.position, boneToJoint.transform.position) <= 0.4f) {
                SetLeashToPlayerHuman();
                puttingOnLeash = false;
            }
        }
        
    }

    private bool DogInRange()
    {
        var currentDistance = Vector2.Distance(playerHuman.transform.position, PlayerController2D.Instance.transform.position);
        if (currentDistance > minLeashDistance) return false;
        return true;
    }

    private void SetLeashToPlayerHuman()
    {
        //exclude the scripts and navmesh (navmesh interferes with hingejoint-leash connection)
        agentEnemyScript.agent.enabled = false;
        agentEnemyScript.enabled = false;
        moveWaypoints.enabled = false;
        //add some linear drag otherwise Vlado will swing around Bosko like yo-yo
        playerHuman.GetComponent<Rigidbody2D>().drag = 2f;
        leash.SetActive(true);  
        joint.enabled = true;
    }

    private void DisconnectLeashFromPlayerHuman()
    {
        leash.SetActive(false);
        joint.enabled = false;
        SFXManager.Instance.PlaySound(SFXManager.Instance.leashDetach);
        //include the scripts and navmesh again
        agentEnemyScript.enabled = true;
        agentEnemyScript.agent.enabled = true;
        moveWaypoints.enabled = true;        
    }

    private bool ClickedOnVlado()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D hitObject = Physics2D.OverlapCircle(position, 0.1f, detectLayer);
            if (hitObject != null)
            {
                Debug.Log("Vlado clicked");
                return true;
            }
        }
  
        return false;
    }
}
