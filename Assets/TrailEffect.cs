using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    private bool ballMoving;

    public float startTimeBtwTrail = 0.1f;
    private float timeBtwTrail;
    public GameObject trailEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrailEffectSpawn();
        if(GetComponent<Rigidbody2D>().velocity.magnitude >= 0.01f) {
            ballMoving = true;
        } else {
            ballMoving = false;
        }
    }

    private void TrailEffectSpawn() {
        if (ballMoving) {
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
}
