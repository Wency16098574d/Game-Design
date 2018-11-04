using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dazhao2Controller : MonoBehaviour {

    private bool heroinrange;
    Health health;
    public float attackDuration = 1f;
    private float cooldownTimer;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        heroinrange = false;
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
//        targetPosition = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            cooldownTimer = attackDuration;
            if (heroinrange)
            {
                health.TakeDamage(5);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, 3 * Time.deltaTime);


        Destroy(transform.gameObject, 10f);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            heroinrange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            heroinrange = false;
        }
    }

}
