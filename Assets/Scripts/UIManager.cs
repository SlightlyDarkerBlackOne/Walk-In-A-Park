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

    private Button_UI [] button = new Button_UI [2];

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

        humanImage = transform.Find("Switch Control").transform.Find("PlayerImage").gameObject.GetComponent<Image>().sprite;
        pickupIndicatiorText.gameObject.SetActive(false);
        pickupIndicatiorTextOriginal = pickupIndicatiorText.text;
        dogImage = PlayerController2D.Instance.transform.Find("Animation").GetComponent<SpriteRenderer>().sprite;
    }


    // Update is called once per frame
    void Update()
    {
        ManageIcons();

        if (!PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            transform.Find("Switch Control").Find("PlayerImage").gameObject.GetComponent<Image>().sprite = dogImage;
        } else {
            transform.Find("Switch Control").Find("PlayerImage").gameObject.GetComponent<Image>().sprite = humanImage;
        }
    }

    void GetIcons()
    {
        button[0] = transform.GetChild(6).GetComponent<Button_UI>(); //pee icon
        button[1] = transform.GetChild(7).GetComponent<Button_UI>(); //scent icon

    }

    void ManageIcons()
    {
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
