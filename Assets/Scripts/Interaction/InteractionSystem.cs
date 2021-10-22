using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection")]
    public Transform detectPoint;
    private const float detectRadius = 0.2f;
    public LayerMask detectLayer;
    public GameObject detectedItem;
    [Header("ExamineItem")]
    public GameObject examinePopUp;
    public Image examineImage;
    public Text examineDescription;
    public bool isExamining = false;


    // Update is called once per frame
    void Update()
    {
        if (DetectItem())
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Detected");
                detectedItem.GetComponent<Item>().Interact();
            }
        }
        
    }

    bool DetectItem()
    {
        Collider2D item = 
            Physics2D.OverlapCircle(detectPoint.position, detectRadius, detectLayer);
        if (item)
        {
            detectedItem = item.gameObject;
            return true;
        }
        else
        {
            detectedItem = null;
            return false;
        }
    }

    public void PickUpInMouth(GameObject item)
    {

    }

    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            examinePopUp.SetActive(false);
            isExamining = false;
        }
        else
        {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineDescription.text = item.itemDescription;
            Debug.Log("Called examine window");
            examinePopUp.SetActive(true);
            isExamining = true;
        }

    }

}
