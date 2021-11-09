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
    private float minLeashDistance;
    public static bool puttingOnLeash;
    private Vector2 previousPosition;
    public LayerMask detectLayer;

    // Start is called before the first frame update
    void Start()
    {
        leash = transform.Find("Leash").gameObject;
        joint = playerHuman.GetComponent<HingeJoint2D>();

        minLeashDistance = Vector2.Distance(playerHuman.transform.position, PlayerController2D.Instance.transform.position);

        puttingOnLeash = false;

        detectLayer = LayerMask.GetMask("Human");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || ClickedOnVlado() == true) 
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
        
        if (puttingOnLeash)
        {
            playerHuman.transform.position = Vector2.Lerp(playerHuman.transform.position,
                boneToJoint.transform.position, 5f*Time.deltaTime);

            if (Vector2.Distance(playerHuman.transform.position, boneToJoint.transform.position)<=0.3f) 
            {
                Debug.Log("close enough");
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
        leash.SetActive(true);  
        joint.enabled = true;
    }

    private void DisconnectLeashFromPlayerHuman()
    {
        leash.SetActive(false);
        joint.enabled = false;
        SFXManager.Instance.PlaySound(SFXManager.Instance.leashDetach);
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
