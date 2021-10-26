using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leash : MonoBehaviour
{
    private GameObject leash;
    public GameObject playerHuman;
    public GameObject boneToJoint;
    private HingeJoint2D joint;
    private float minLeashDistance;
    public static bool PuttingOnLeash;
    private Vector2 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        leash = transform.Find("Leash").gameObject;
        joint = playerHuman.GetComponent<HingeJoint2D>();

        minLeashDistance = Vector2.Distance(playerHuman.transform.position, PlayerController2D.Instance.transform.position);

        PuttingOnLeash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            if (!leash.activeSelf && DogInRange())
            {
                PuttingOnLeash = true;
            }
            else if (leash.activeSelf)
            {
                DisconnectLeashFromPlayerHuman();
            }
        }
        
        if (PuttingOnLeash)
        {
            playerHuman.transform.position = Vector2.Lerp(playerHuman.transform.position,
                boneToJoint.transform.position, 5f*Time.deltaTime);

            if (Vector2.Distance(playerHuman.transform.position, boneToJoint.transform.position)<=0.1f) 
            {
                Debug.Log("close enough");
                SetLeashToPlayerHuman();
                PuttingOnLeash = false;
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
        leash.SetActive(true);  
        joint.enabled = true;
    }

    private void DisconnectLeashFromPlayerHuman()
    {
        leash.SetActive(false);
        joint.enabled = false;
    }
}
