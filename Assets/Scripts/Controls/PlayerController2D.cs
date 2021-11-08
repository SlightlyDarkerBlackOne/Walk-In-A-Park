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
    private const float detectRadius = 0.3f;
    public LayerMask detectLayer;
    public GameObject detectedItem;
    public List<GameObject> cleanUpList = new List<GameObject>();
    public bool carryItem = false;

    public float screenWidth;
    public float screenHeight;
    private float horizontal = 0f;
    private float vertical = 0f;

    float moveX;
    float moveY;

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
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
    #endregion

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
            if (touch.position.x < screenWidth/4 || 
                touch.position.x > 3*screenWidth/4){
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
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                carryItem = !carryItem;
                if (!carryItem)
                {
                    Vector2 force = new Vector2(lastMoveDir.x*5f, lastMoveDir.y*5f);
                    Rigidbody2D rbItem = detectedItem.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                    rbItem.gravityScale = 0f;
                    rbItem.drag = 2f;
                    rbItem.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rbItem.AddForce(force, ForceMode2D.Impulse);
                    //add the item to the list of objects to remove rigidbody2d from later
                    cleanUpList.Add(detectedItem);                       
                }

            }

            //mobile
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {                
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Collider2D obj = Physics2D.OverlapCircle(position, detectRadius, detectLayer); 
                if (obj)
                {
                    carryItem = !carryItem;
                    //if carryItem becomes false, we are about to throw it
                    if (!carryItem)
                    {
                        //only when throwing - add rigidbody2d, adjust params, throw
                        //if we had rigidbody2d while carrying, if kinematic would block player from moving in its direction
                        //if dynamic wouldn't work with following via transform at all
                        Vector2 force = new Vector2(lastMoveDir.x*5f, lastMoveDir.y*5f);
                        Rigidbody2D rbItem = detectedItem.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                        rbItem.gravityScale = 0f;
                        rbItem.drag = 1.5f;
                        rbItem.constraints = RigidbodyConstraints2D.FreezeRotation;
                        rbItem.AddForce(force, ForceMode2D.Impulse);
                        //add the item to the list of objects to remove rigidbody2d from later
                        cleanUpList.Add(detectedItem);                       
                    }
                }                
            }            
        }

        if (carryItem) {
            detectedItem.transform.parent = transform;
            UIManager.Instance.HidePickupIndicatorText();
        } else if (DetectItem()) detectedItem.transform.parent = null;

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
            UIManager.Instance.ShowPickupIndicatorText(item.name);
            detectedItem = item.gameObject;
            return true;
        }
    }   
}