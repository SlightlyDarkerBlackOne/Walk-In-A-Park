using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSortByYAxisToChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            if(child.GetComponent<Renderer>() != null) {
                child.gameObject.AddComponent<SortByYAxis>();
                SortByYAxis sortByYAxis = child.gameObject.GetComponent<SortByYAxis>();
                sortByYAxis._precision = 0.1f;
            }   
        }
    }
}
