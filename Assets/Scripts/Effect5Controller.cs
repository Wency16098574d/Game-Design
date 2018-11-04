using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect5Controller : MonoBehaviour {
    private GameObject Player;
    // Use this for initialization
    void Start () {

       
    Player = GameObject.FindGameObjectWithTag("Player");
}
	
	// Update is called once per frame
	void Update () {
        //       var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
        //       transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
        transform.LookAt(transform.position*2-Player.transform.position);
    }
}
