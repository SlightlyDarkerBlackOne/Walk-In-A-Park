using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leash : MonoBehaviour
{
    public GameObject leash;
    public GameObject playerHuman;
    public GameObject playerDog;
    public HingeJoint2D joint;
    public GameObject boneToJoint;
    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        joint = playerHuman.GetComponent<HingeJoint2D>();

        dist = Vector2.Distance(playerHuman.transform.position, playerDog.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            if (!leash.activeSelf && DogInRange())
            {
                leash.SetActive(true);
                //move the end of leash from where it was left off to the new position 
                //of the human before reactivating? (for now just teleport the player to that location)
                playerHuman.transform.position = boneToJoint.transform.position;
                
                joint.enabled = true;
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
        var d = Vector2.Distance(playerHuman.transform.position, playerDog.transform.position);
        if (d > dist) return false;
        return true;
    }
}
