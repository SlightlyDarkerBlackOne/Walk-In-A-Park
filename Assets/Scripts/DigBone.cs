using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigBone : MonoBehaviour
{
    public bool digging = false;
    [SerializeField]
    private ParticleSystem digParticle;
    
    [SerializeField]
    private float digDuration;
    public LayerMask detectLayer, detectLayer2;
    public Sprite dugOutDirtPile;

    // Start is called before the first frame update
    void Start()
    {
        detectLayer = LayerMask.GetMask("Layer 1");
        detectLayer2 = LayerMask.GetMask("Item");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!digging && ClickedDigIcon()) {
            StartCoroutine(Digging());
        }
       
        
    }

    bool ClickedDigIcon()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D digSpot = Physics2D.OverlapCircle(position, 0.3f, detectLayer); 
            if (digSpot != null && digSpot.name == "DirtPile")
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator Digging() {
        digging = true;
        digParticle.Play();
        //dig = Instantiate(digParticle, Vector2.zero, Quaternion.Euler(0, 0, -118f)) as GameObject;
        //dig.transform.position = transform.position;

        PlayerController2D.Instance.FrezePlayer();
        yield return new WaitForSeconds(digDuration);
        PlayerController2D.Instance.UnFreezePlayer();
        //Destroy(dig);
        digParticle.Stop();
        //if (QuestManager.Instance.questCompleted[2]) GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(3);
        Collider2D bone = Physics2D.OverlapCircle(transform.position, 1f, detectLayer2); 
        if (bone != null && bone.name == "BoskoBone")
        {
            bone.gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = dugOutDirtPile;
            GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(4);
        }
        digging = false;
    }
}

