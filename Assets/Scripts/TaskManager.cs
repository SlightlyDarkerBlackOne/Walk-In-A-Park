using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    //task 2 (territory)
    Dictionary<string,bool> areaDict = new Dictionary<string,bool>();
    public List<GameObject> peeList = new List<GameObject>();
    private List<GameObject> toDelete = new List<GameObject>();
    private Animator anim;
    public LayerMask detectLayer;
    

    // Start is called before the first frame update
    void Start()
    {
        areaDict.Add("Playground", false);
        areaDict.Add("Area1", false);
        areaDict.Add("Area2", false);
        areaDict.Add("Area3", false);
        anim = GetComponent<Animator>();   
        detectLayer = LayerMask.GetMask("Area");     
    }
    public bool CheckTaskOnToDoList() {
        if (Scent.ballFound || true) {
            StartCoroutine(CheckTaskOnList(1));
            return true;
        }
        return false;
    }
    public bool CheckTaskOnToDoList5()
    {
        foreach (var pee in peeList)
        {
            //area layer
            Collider2D area = Physics2D.OverlapCircle(pee.transform.position, 0.1f, detectLayer);

            if (area != null)
            {
                Debug.Log(area.name);
                areaDict[area.name] = true;   
                Debug.Log(area.name);                          
            }

            toDelete.Add(pee);
        }
        
        foreach (GameObject pee in toDelete)
        {
            peeList.Remove(pee);
        }

        foreach (var area in areaDict)
        {
            if (area.Value == false) return false;
        }
        
        StartCoroutine(CheckTaskOnList(5));
        return true;
    }
    //changes the toggle's Normal colour to the new colour
    IEnumerator CheckTaskOnList(int taskNumber) {
        GameObject task = gameObject.transform.GetChild(1).GetChild(taskNumber - 1).GetChild(2).gameObject;
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("isShowing", true);
        yield return new WaitForSeconds(0.8f);
        task.GetComponent<Animator>().SetTrigger("show");
        //task.GetComponent<Toggle>().isOn = true;
        //ColorBlock cb = task.GetComponent<Toggle>().colors;
        //cb.normalColor = Color.black;
        //task.GetComponent<Toggle>().colors = cb;
    }
}
