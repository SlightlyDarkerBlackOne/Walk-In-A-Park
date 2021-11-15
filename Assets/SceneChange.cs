using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void SceneLoad()
    {
        Debug.Log("Scene change");
        SceneManager.LoadScene(0);
    }

}
