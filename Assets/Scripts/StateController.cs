using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {
    public float bossHealth=200;
    public float changeTo2 = 0.6f;
    public float changeTo3 = 0.3f;
    private bool change;
    private bool wudi;
    public float currentbossHealth;
    private Animator animator;
    BossController bossController;
	// Use this for initialization
	void Start () {
        currentbossHealth = bossHealth;
        bossController = GetComponent<BossController>();
        animator = GetComponent<Animator>();
        change = true;
        wudi = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (currentbossHealth > bossHealth * changeTo2)
        {
            bossController.state1 = true;
        }
        else if (change)
        {
            wudi = true;
            bossController.state1 = false;
            bossController.state1to2 = true;
            StartCoroutine(ChangeTo2());
            change = false;
        }
        else if(currentbossHealth < bossHealth * changeTo3 && bossController.state2)
        {
            bossController.state2 = false;
            bossController.state2to3 = true;
            wudi = true;
            StartCoroutine(ChangeTo3());
        }
        else if (currentbossHealth <= 0)
        {
            bossController.state3 = false;
            bossController.isDead = true;
        }
	}


    IEnumerator ChangeTo2()
    {

        yield return new WaitForSeconds(20f);
        
        bossController.state1to2 = false;
        bossController.state2 = true;
        wudi = false;

    }
    IEnumerator ChangeTo3()
    {

        yield return new WaitForSeconds(20f);

        bossController.state2to3 = false;
        bossController.state3 = true;
        wudi = false;

    }

    public void TakeDamage(int a)
    {
        if (!wudi)
        {
            currentbossHealth -= a;
        }
      
        
    }
}
