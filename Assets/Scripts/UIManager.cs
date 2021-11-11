using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private Sprite humanImage;
    private Sprite dogImage;
    public TextMeshProUGUI pickupIndicatiorText;
    public TextMeshProUGUI clickClackIndicatiorText;

    #region Singleton
    public static UIManager Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    private string pickupIndicatiorTextOriginal;
    // Start is called before the first frame update
    void Start()
    {
        humanImage = transform.Find("Switch Control").transform.Find("PlayerImage").gameObject.GetComponent<Image>().sprite;
        pickupIndicatiorText.gameObject.SetActive(false);
        pickupIndicatiorTextOriginal = pickupIndicatiorText.text;
        dogImage = PlayerController2D.Instance.transform.Find("Animation").GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            transform.Find("Switch Control").Find("PlayerImage").gameObject.GetComponent<Image>().sprite = dogImage;
        } else {
            transform.Find("Switch Control").Find("PlayerImage").gameObject.GetComponent<Image>().sprite = humanImage;
        }
    }

    public void ShowPickupIndicatorText(string name) {
        pickupIndicatiorText.gameObject.SetActive(true);
        pickupIndicatiorText.text = pickupIndicatiorTextOriginal + name;
        pickupIndicatiorText.GetComponent<Animator>().SetBool("itemInRange", true);   
    }

    public void HidePickupIndicatorText() {
        pickupIndicatiorText.gameObject.SetActive(false);
        pickupIndicatiorText.GetComponent<Animator>().SetBool("itemInRange", false);
    }

    public void ShowClickClackText() {
        clickClackIndicatiorText.gameObject.SetActive(true);
        clickClackIndicatiorText.text = "Press E to Click-Clack";
        clickClackIndicatiorText.GetComponent<Animator>().SetBool("itemInRange", true);
    }
    public void ShowStopClickClackText() {
        clickClackIndicatiorText.text = "Press E to Stop Click-Clacking";
    }
    public void HideClickClackIndicatorText() {
        clickClackIndicatiorText.gameObject.SetActive(false);
        clickClackIndicatiorText.GetComponent<Animator>().SetBool("itemInRange", false);
    }
}
