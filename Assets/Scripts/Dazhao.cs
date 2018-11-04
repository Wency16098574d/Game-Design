using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dazhao : MonoBehaviour {
    private bool heroinrange;
    Health health;
    private bool flag;
	// Use this for initialization
	void Start () {
        heroinrange = false;
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        flag = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (flag)
        {
            StartCoroutine(AttackCheck());
            flag = false;
            Destroy(transform.gameObject, 9f);
        }
       
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
    IEnumerator AttackCheck()
    {
        yield return new WaitForSeconds(5.5f);
        if (heroinrange)
        {
            health.TakeDamage(5);
        }
       
    }
}
