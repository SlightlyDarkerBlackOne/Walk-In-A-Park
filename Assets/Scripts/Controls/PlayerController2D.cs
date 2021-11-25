using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Follow,
    }

    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private Animator anim;
    private State state;

    public float startTimeBtwTrail;
    private float timeBtwTrail;
    public GameObject trailEffect;

    private bool playerMoving;
    public bool playerFrozen = false;

    public GameObject leash;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    public Transform detectPoint;
    private const float detectRadius = 0.9f;
    private const float detectRadiusThrow = 0.2f;
    public LayerMask detectLayer;
    public GameObject detectedItem;
    public List<GameObject> cleanUpList = new List<GameObject>();
    public bool carryItem = false;
    [SerializeField]
    private Vector3 carryOffset;
    private float carryOffsetX = 0.9f;

    public float screenWidth;
    public float screenHeight;
    public float horizontal = 0f;
    public float vertical = 0f;

    float moveX;
    float moveY;

    public float throwForce = 70f;
    public float throwDrag = 16f;

    private Vector2 playerTransformBeforeMoving;
    public AgentEnemy agentEnemyScript;
    private bool boneFound = false;

    #region Singleton
    public static PlayerController2D Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        anim = transform.Find("Animation").GetComponent<Animator>();
        state = State.Walking;

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
    
        detectLayer = LayerMask.GetMask("Item");
        detectPoint = gameObject.transform;


        agentEnemyScript = GameObject.FindWithTag("Human").GetComponent<AgentEnemy>();
    }
    #endregion

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

    }

    // Update is called once per frame
    void Update() 
    {
        Move();
        Interact();
        SetAnimations();
        TrailEffect();

        if (playerFrozen) {
            state = State.Idle;
            playerMoving = false;
        }

    }

    private void FixedUpdate() 
    {
        switch (state) {
            case State.Idle:
                rb.velocity = Vector2.zero;
                break;
            case State.Walking:
                rb.velocity = moveDir * moveSpeed;
                break;
        }
    }

    private void Move() 
    {
        switch (state) {
            case State.Idle:
                if (!playerFrozen)
                    state = State.Walking;
                break;
            case State.Walking:
                moveX = 0f;
                moveY = 0f;

                WASDMovement();
                TouchMovement();

                moveDir = new Vector3(moveX, moveY).normalized;
                break;

            case State.Follow:

                if (!leash.activeSelf) 
                {
                    state = State.Idle;
                }
                Follow();
                break;
        }
    }
    private void TouchMovement() {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))){
                    vertical = 0f;
                }

        if (moveX != 0 || moveY != 0) {
            playerMoving = true;
            lastMoveDir = moveDir;
        } 
        else {
            playerMoving = false;
            horizontal = 0f;
            vertical = 0f;
        }

        Vector2 target;
        if (Input.touchCount > 0){

            Touch touch = Input.GetTouch(0);

            if (touch.position.y > screenHeight / 2)
                vertical = 1.0f;
            
            if (touch.position.y < screenHeight / 2)
                vertical = -1.0f;

            if (touch.position.x > screenWidth / 2)
                horizontal = 1.0f;
            
            if (touch.position.x < screenWidth / 2)
                horizontal = -1.0f;

            if (touch.position.y < 2 * screenHeight / 3 &&
                touch.position.y > screenHeight / 3) {
                vertical = 0f;
            }
            if (touch.position.x < screenWidth/6 || 
                touch.position.x > 5*screenWidth/6){
                vertical = 0f;
            }
                    
            target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var delta = 2f*moveSpeed*Time.deltaTime;
            Vector2 position = Vector3.MoveTowards(transform.position, target, delta);    
            rb.MovePosition(position);
            lastMoveDir = new Vector2 (horizontal, vertical);
            playerMoving = true;

        } 
        else if (moveX == 0 && moveY == 0) 
        { 
            horizontal = 0f; 
            vertical = 0f; 
            playerMoving = false;
        }
    }

    public void SetPlayerToLocationAndFreeze(Transform transformToSet) {
        playerTransformBeforeMoving = transform.position;
        transform.position = transformToSet.position;
        FrezePlayer();
    }
    public void SetPlayerToLocation(Transform transformToSet) {
        transform.position = transformToSet.position;
    }
    public void RemovePlayerFromLocationAndUnfreeze() {
        transform.position = playerTransformBeforeMoving;
        UnFreezePlayer();
    }
    private void WASDMovement() {
        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
            vertical = 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
            vertical = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
            horizontal = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
            horizontal = 1f;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))) {
            vertical = 0f;
        }

        if (moveX != 0 || moveY != 0) {
            playerMoving = true;
            lastMoveDir = moveDir;
        } else {
            playerMoving = false;
            horizontal = 0f;
            vertical = 0f;
        }
    }
    private void Follow() {

        if (playerMoving) {

            timeToMoveCounter -= Time.deltaTime;
            rb.velocity = moveDir;

            //Ako je dosao do ruba zone za hodanje
            //IsOverTheZone();

            if (timeToMoveCounter < 0f) {
                playerMoving = false;
                anim.SetBool("PlayerMoving", false);
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                lastMoveDir = moveDir;
            }

        } else {
            timeBetweenMoveCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;

            if (timeBetweenMoveCounter < 0f) {
                playerMoving = true;
                anim.SetBool("PlayerMoving", true);
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

                moveDir = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
        }
    }

    private void SetAnimations() {
        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMoveDir.x);
        anim.SetFloat("LastMoveY", lastMoveDir.y);
    }
    private void TrailEffect() {
        if (playerMoving) {
            if (timeBtwTrail <= 0) {
                GameObject effect = Instantiate(trailEffect, transform.position, Quaternion.identity);
                effect.transform.parent = this.transform;
                Destroy(effect, 2f);
                timeBtwTrail = startTimeBtwTrail;
            } else {
                timeBtwTrail -= Time.deltaTime; ;
            }
        }
    }
    public void FrezePlayer() {
        playerFrozen = true;
    }
    public void UnFreezePlayer() {
        playerFrozen = false;
    }

    private void Interact()
    {       
        //carrying items in mouth
        if (DetectItem())
        {
            //PC
            if (Input.GetKeyDown(KeyCode.E) && detectedItem.GetComponent<Rigidbody2D>() == null) 
            {
                carryItem = !carryItem;
                if (!carryItem)
                {
                    //ThrowItem(); 
                                       
                }
               

            }

            //mobile
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {                
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Collider2D obj = Physics2D.OverlapCircle(position, detectRadiusThrow, detectLayer); 
                if (obj)
                {
                    carryItem = !carryItem;
                    //if carryItem becomes false, we are about to throw it
                    if (!carryItem)
                    {
                        //ThrowItem();   
                                          
                    }
                    
                }                
            }            
        }

        if (carryItem) {
            if (detectedItem == null) carryItem = false;
            if (detectedItem != null) detectedItem.GetComponent<Collider2D>().isTrigger = true;

            switch(horizontal)
            {
                case -1: 
                    carryOffset = new Vector3(-carryOffsetX,-0.1f,0); 
                    detectedItem.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 2");
                    break;
                case 1: 
                    carryOffset = new Vector3(carryOffsetX,-0.1f,0); 
                    detectedItem.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 2");
                    break;
            }
            switch(vertical)
            {
                case -1: 
                    carryOffset = new Vector3(0,-0.3f,0); 
                    detectedItem.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 2");
                    break;
                case 1: 
                    carryOffset = new Vector3(0, 0.3f,0);
                    detectedItem.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 1");
                    break;
            }
            
            
            //detectedItem.transform.parent = transform;
            if (detectedItem != null) detectedItem.transform.position = transform.position+carryOffset;
            UIManager.Instance.HidePickupIndicatorText();
        } //else if (DetectItem()) detectedItem.transform.parent = null;
        else if (DetectItem())
        {
            detectedItem.GetComponent<Collider2D>().isTrigger = false;
            detectedItem.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 1"); 
        }

        //remove added rigidbodies2d so that those items can be carried&thrown again
        List<GameObject> toDelete = new List<GameObject>();
        foreach (GameObject item in cleanUpList)
        {
            //qualifies only if it had rb added and is not moving anymore (not in process of being thrown)
            if (item.GetComponent<Rigidbody2D>() != null && item.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                Destroy(item.GetComponent<Rigidbody2D>());
                toDelete.Add(item);
                Debug.Log("Removed rb");
            }
        }
        foreach (GameObject item in toDelete)
        {
            cleanUpList.Remove(item);
            Debug.Log("Deleted");
        }

        //call for Vlado by barking
        if (Input.GetKeyDown(KeyCode.B) || ClickedOnDog()) 
        {
            //play barking sound
            //SFXManager.Instance.PlaySound(SFXManager.Instance.);
            agentEnemyScript.followPlayer = true;
        }

    }

    private bool ClickedOnDog()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D hitObject = Physics2D.OverlapCircle(position, 0.1f);
            if (hitObject != null && hitObject.tag == "Player")
            {
                Debug.Log("Bosko clicked");
                return true;
            }
        }
  
        return false;
    }

    private bool DetectItem()
    {
        Collider2D item = Physics2D.OverlapCircle(detectPoint.position,
            detectRadius, detectLayer);

        if (item == null)
        {
            UIManager.Instance.HidePickupIndicatorText();
            detectedItem = null;
            return false;
        }
        else
        {
            //temporary
            if (item.name == "BoskoBone" && !boneFound) 
            {
                GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(3);
                boneFound = true;

            }

            UIManager.Instance.ShowPickupIndicatorText(item.name);
            detectedItem = item.gameObject;
            return true;
        }
    }

    //only when throwing - add rigidbody2d, adjust params, throw
    //if we had rigidbody2d while carrying, if kinematic would block player from moving in its direction
    //if dynamic wouldn't work with following via transform at all
    private void ThrowItem() {
        Vector2 force = new Vector2(lastMoveDir.x * throwForce, lastMoveDir.y * throwForce);
        Rigidbody2D rbItem = detectedItem.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        rbItem.gravityScale = 0f;
        rbItem.drag = throwDrag;
        rbItem.constraints = RigidbodyConstraints2D.FreezeRotation;
        rbItem.AddForce(force, ForceMode2D.Impulse);
        SFXManager.Instance.PlaySound(SFXManager.Instance.itemThrow);
        //add the item to the list of objects to remove rigidbody2d from later
        cleanUpList.Add(detectedItem);
    }

    //temporary lol
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Milica") 
        {
            GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(2);
        }
    }
}