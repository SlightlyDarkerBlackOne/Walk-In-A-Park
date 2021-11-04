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
    public bool carryItem = false;

    public float screenWidth;
    public float screenHeight;
    private float horizontal;
    private float vertical;

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
        Vector2 target;
        if (Input.touchCount > 0) {

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

            target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var delta = 2f * moveSpeed * Time.deltaTime;
            Vector2 position = Vector3.MoveTowards(transform.position, target, delta);
            rb.MovePosition(position);
            lastMoveDir = new Vector2(horizontal, vertical);
            playerMoving = true;

        } else if (moveX == 0 && moveY == 0) {
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
        if (DetectItem())
        {
            //PC
            if (Input.GetKeyDown(KeyCode.G)) carryItem = !carryItem;

            //mobile
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Collider2D obj = Physics2D.OverlapCircle(position, detectRadius, detectLayer); 
                if (obj)
                {
                    carryItem = !carryItem;
                }
                
            }
        }

        if (carryItem) detectedItem.transform.parent = transform;
        else if (DetectItem()) detectedItem.transform.parent = null;
    }

    private bool DetectItem()
    {
        Collider2D item = Physics2D.OverlapCircle(detectPoint.position,
            detectRadius, detectLayer);

        if (item == null)
        {
            detectedItem = null;
            return false;
        }
        else
        {
            detectedItem = item.gameObject;
            return true;
        }
    }   

}