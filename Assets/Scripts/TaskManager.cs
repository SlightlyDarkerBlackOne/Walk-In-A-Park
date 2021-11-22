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
    

    // Start is called before the first frame update
    void Start()
    {
        areaDict.Add("Playground", false);
        areaDict.Add("Area1", false);
        areaDict.Add("Area2", false);
        areaDict.Add("Area3", false);
        anim = GetComponent<Animator>();        
    }
    public bool CheckTaskOnToDoList() {
        if (Scent.ballFound || true) {
            CheckTaskOnList(1);
            return true;
        }
        return false;
    }
    public bool Task2()
    {
        foreach (var pee in peeList)
        {
            //area layer
            Collider2D area = Physics2D.OverlapCircle(pee.transform.position, 0.1f);

            if (area != null)
            {
                Debug.Log(area.name);
                areaDict[area.name] = true;                             
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

        CheckTaskOnList(2);
        return true;
    }
    //changes the toggle's Normal colour to the new colour
    void CheckTaskOnList(int taskNumber) {
        GameObject task = gameObject.transform.GetChild(1).GetChild(taskNumber - 1).gameObject;
        task.GetComponent<Toggle>().isOn = true;
        ColorBlock cb = task.GetComponent<Toggle>().colors;
        cb.normalColor = Color.black;
        task.GetComponent<Toggle>().colors = cb;
        anim.SetBool("isShowing", true);
    }
}
