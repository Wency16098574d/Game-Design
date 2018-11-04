using UnityEngine;
using System.Collections;

public class MoveCTRLDemo : MonoBehaviour {

	private float move = 20;
	private bool stop = false;	
	private float blend;
	private float delay = 0;
	public float AddRunSpeed = 1;
	public float AddWalkSpeed = 1;
	private bool hasAniComp = false;

	// Use this for initialization
	void Start () 
	{
	
		if ( null != GetComponent<Animation>() )
		{
			hasAniComp = true;
		}

	}

	void Move ()
	{ 
		float speed =0.0f;
		float add =0.0f;

		if ( hasAniComp == true )
		{	
//			if ( Input.GetKey(KeyCode.W) )
            if(Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0)
			{
                //				move *= 1.015F;

                //				if ( move>250 && CheckAniClip( "move_forward_fast" )==true )
                Vector3 movement;
                if (CheckAniClip("move_forward_fast") == true)
                {
					{
						GetComponent<Animation>().CrossFade("move_forward_fast");
                        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
						add = 5*AddRunSpeed;
                        transform.Translate(movement*Time.deltaTime*10);
                    }
				}
//				else
//				{
//					GetComponent<Animation>().Play("move_forward");
//					add = 5*AddWalkSpeed;
//				}
//
//				speed = Time.deltaTime*add;

				
			}


			if ( Input.GetKeyUp(KeyCode.W))
			{
//				if ( GetComponent<Animation>().IsPlaying("move_forward"))
//				{	GetComponent<Animation>().CrossFade("idle_normal",0.3f); }
				if ( GetComponent<Animation>().IsPlaying("move_forward_fast"))
				{	
					GetComponent<Animation>().CrossFade("idle_normal");
					stop = true;
				}	
				move = 20;
			}
            /*
                        if (stop == true)
                        {	
                            float max = Time.deltaTime*20*AddRunSpeed;
                            blend = Mathf.Lerp(max,0,delay);

                            if ( blend > 0 )
                            {	
                                transform.Translate( 0,0,blend );
                                delay += 0.025f; 
                            }	
                            else 
                            {	
                                stop = false;
                                delay = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        if ( Input.GetKey(KeyCode.W) )
                        {  	
                            add = 5*AddWalkSpeed;
                            speed = Time.deltaTime*add;
                            transform.Translate( 0,0,speed );
                        }
            */
        }

    }

	bool CheckAniClip ( string clipname )
	{	
		if( this.GetComponent<Animation>().GetClip(clipname) == null ) 
			return false;
		else if ( this.GetComponent<Animation>().GetClip(clipname) != null ) 
			return true;

		return false;
	}

	// Update is called once per frame
	void Update () 
	{
	
		Move();

        Vector3 targetPoint = Camera.main.transform.forward * 100;
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint);


        if ( hasAniComp == true )
		{	
			if (Input.GetKey(KeyCode.V))
			{	
				if ( CheckAniClip( "dead" ) == false ) return;

				GetComponent<Animation>().CrossFade("dead",0.2f);
				//					animation.CrossFadeQueued("idle_normal");
			} 



			if (Input.GetKey(KeyCode.Q))
			{	
				if ( CheckAniClip( "attack_short_001" ) == false ) return;

				GetComponent<Animation>().CrossFade("attack_short_001",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_combat");
			}



			if (Input.GetKey(KeyCode.Z))
			{	
				if ( CheckAniClip( "damage_001" ) == false ) return;

				GetComponent<Animation>().CrossFade("damage_001",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_combat");
			}

		

			if (Input.GetKey(KeyCode.D))
			{	
				if ( CheckAniClip( "idle_normal" ) == false ) return;

				GetComponent<Animation>().CrossFade("idle_normal",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_normal");			
			}			

			if (Input.GetKey(KeyCode.F))
			{	
				if ( CheckAniClip( "idle_combat" ) == false ) return;

				GetComponent<Animation>().CrossFade("idle_combat",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_normal");			
			}			
		}

		if ( Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate( 0,Time.deltaTime*-100,0);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(0,Time.deltaTime*100,0);
		}

	}
}
