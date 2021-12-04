using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDialogues : MonoBehaviour
{
    #region Singleton
    public static RandomDialogues Instance { get; private set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    public Dialogue boskoConcernedDialogue;
}
