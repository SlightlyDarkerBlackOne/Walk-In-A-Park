using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class UIManager : MonoBehaviour
{
    private Sprite humanImage;
    private Sprite dogImage;
    public TextMeshProUGUI pickupIndicatiorText;
    public TextMeshProUGUI clickClackIndicatiorText;

    private Button_UI[] button = new Button_UI[2];

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
        GetIcons();
        pickupIndicatiorText.gameObject.SetActive(false);
        pickupIndicatiorTextOriginal = pickupIndicatiorText.text;
        dogImage = PlayerController2D.Instance.transform.Find("Animation").GetComponent<SpriteRenderer>().sprite;
    }
    void Update() {
        ManageIcons();
    }

    void GetIcons() {
        button[0] = transform.Find("PeeButton").GetComponent<Button_UI>(); //pee icon
        button[1] = transform.Find("ScentButton").GetComponent<Button_UI>(); //scent icon
    }

    void ManageIcons() {
        button[0].GetComponent<Button_UI>().ClickFunc = () => {
            Pissing.ClickedPeeIcon = true;
        };
        button[1].GetComponent<Button_UI>().ClickFunc = () => {
            Scent.ClickedScentIcon = true;
        };

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
