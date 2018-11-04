using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        if (tm != null)
        {
            tm.CollisionEnter += Tm_CollisionEnter;
        }
    }
    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
    {
        Debug.Log(e.Hit.transform.name); //will print collided object name to the console.
    }
}
