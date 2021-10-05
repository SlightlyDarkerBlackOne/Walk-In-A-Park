using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leash : MonoBehaviour
{
    public GameObject leash;
    public GameObject playerHuman;
    public GameObject boneToJoint;
    private HingeJoint2D joint;
    private float minLeashDistance;

    // Start is called before the first frame update
    void Start()
    {
        joint = playerHuman.GetComponent<HingeJoint2D>();

        minLeashDistance = Vector2.Distance(playerHuman.transform.position, PlayerController2D.Instance.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            if (!leash.activeSelf && DogInRange())
            {
                SetLeashToPlayerHuman();
            }
            else if (leash.activeSelf)
            {
                leash.SetActive(false);
                joint.enabled = false;
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
        //move the end of leash from where it was left off to the new position 
        //of the human before reactivating? (for now just teleport the player to that location)
        playerHuman.transform.position = boneToJoint.transform.position;
                
        joint.enabled = true;
    }
}
