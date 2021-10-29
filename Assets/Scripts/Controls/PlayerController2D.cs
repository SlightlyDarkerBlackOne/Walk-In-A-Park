using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walking,
        Rolling,
        Follow,
    }

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashSpeed = 2;
    [SerializeField]
    private float rollSpeed = 3.64f;
    [SerializeField]
    private float rollSpeedMinimum = 2.04f;
    [SerializeField]
    private float rollSpeedDropMultiplier = 3.61f;
    private float rollSpeedOngoing;
    [Space(20)]
    [SerializeField]
    private LayerMask dashLayerMask;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private Vector3 rollDir;

    private Animator anim;
    private State state;

    public float startTimeBtwTrail;
    private float timeBtwTrail;
    public GameObject trailEffect;

    private float dashTime;
    public float startDashTime;

    private bool playerMoving;
    private bool isDashButtonDown;
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
    }
    #endregion

    // Update is called once per frame
    void Update() 
    {
        Move();
        SetAnimations();
        TrailEffect();

        if (Input.GetKeyDown(KeyCode.F)) {
            isDashButtonDown = true;
        }
        if (dashTime >= 0) {
            isDashButtonDown = false;
        }
        dashTime -= Time.deltaTime;

        if (playerFrozen) {
            state = State.Idle;
            playerMoving = false;
        }

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

    bool LeashOn()
    {
        bool canMove;
        if (!leash.activeSelf) canMove = true;
        else canMove = false;
        return canMove;
    }

    private void FixedUpdate() 
    {
        switch (state) {
            case State.Idle:
                rb.velocity = Vector2.zero;
                break;
            case State.Walking:
                rb.velocity = moveDir * moveSpeed;
                Dash();
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeedOngoing;
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
                float moveX = 0f;
                float moveY = 0f;

                if (leash.activeSelf) {
                    state = State.Follow;
                }
                
                if (Input.GetKey(KeyCode.W)) {
                    moveY = +1f;
                }
                if (Input.GetKey(KeyCode.S)) {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.A)) {
                    moveX = -1f;
                }
                if (Input.GetKey(KeyCode.D)) {
                    moveX = +1f;
                }
                if (moveX != 0 || moveY != 0) {
                    playerMoving = true;
                    lastMoveDir = moveDir;
                } else {
                    playerMoving = false;
                }
                

                moveDir = new Vector3(moveX, moveY).normalized;

                if (Input.GetKeyDown(KeyCode.Space)) {
                    rollDir = lastMoveDir;
                    rollSpeedOngoing = rollSpeed;
                    if (dashTime <= 0) {
                        SFXManager.Instance.PlaySound(SFXManager.Instance.dash);
                        state = State.Rolling;
                    }
                }
                break;
            case State.Rolling:
                Roll();
                break;
            case State.Follow:
                if (!leash.activeSelf) {
                    state = State.Idle;
                }
                Follow();
                break;
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

    private void Roll() {
        rollSpeedOngoing -= rollSpeedOngoing * rollSpeedDropMultiplier * Time.deltaTime;
        if (rollSpeedOngoing < rollSpeedMinimum) {
            state = State.Walking;
        }
    }
    private void Dash() {
        if (isDashButtonDown && dashTime <= 0) {
            dashTime = startDashTime;
            Vector3 dashPosition = transform.position + lastMoveDir * dashSpeed;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDir,
                    dashSpeed, dashLayerMask);
            if (raycastHit2D.collider != null) {
                dashPosition = raycastHit2D.point;
            }

            rb.MovePosition(dashPosition);

            SFXManager.Instance.PlaySound(SFXManager.Instance.dash);
            isDashButtonDown = false;
        }
    }

    private void SetAnimations() {
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
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
/*
if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    {
        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Debug.Log("Something Hit");
            if (raycastHit.collider.name == "Soccer")
            {
                Debug.Log("Soccer Ball clicked");
            }

            //OR with Tag

            if (raycastHit.collider.CompareTag("SoccerTag"))
            {
                Debug.Log("Soccer Ball clicked");
            }
        }
    }
    */