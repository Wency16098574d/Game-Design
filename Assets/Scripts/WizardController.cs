using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardController : MonoBehaviour {

    Animator animator;
    public float speed=3f;
    public GameObject Effect;
    public GameObject Effect1;
    public Transform AttachPoint;
    public GameObject EffectParent;
    Health health;
    bool isdead=false;
    bool flag = true;

    private float cooldownTimer;
    public float SpeedUpDuration = 1.5f;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }
	
	// Update is called once per frame
	void Update () {
        cooldownTimer -= Time.deltaTime;
        if (health.currentheroHealth > 0 && isdead==false)
        {
            Move();
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 targetPoint;
                animator.SetTrigger("IsAttack");
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))//如果射线碰撞到物体
                {
                    targetPoint = hitInfo.point;//记录碰撞的目标点
                }
                else//射线没有碰撞到目标点
                {
                    //将目标点设置在摄像机自身前方1000米处
                    targetPoint = Camera.main.transform.forward * 1000;
                }
                GameObject effect = Instantiate(Effect, AttachPoint.position, AttachPoint.rotation) as GameObject;
                effect.transform.LookAt(targetPoint);
                effect.transform.parent = EffectParent.transform;
                Destroy(effect, 5);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Vector3 targetPoint;
                animator.SetTrigger("IsAttack");
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))//如果射线碰撞到物体
                {
                    targetPoint = hitInfo.point;//记录碰撞的目标点
                }
                else//射线没有碰撞到目标点
                {
                    //将目标点设置在摄像机自身前方1000米处
                    targetPoint = Camera.main.transform.forward * 1000;
                }
                GameObject effect = Instantiate(Effect1, AttachPoint.position, AttachPoint.rotation) as GameObject;
                effect.transform.LookAt(targetPoint);
                effect.transform.parent = EffectParent.transform;
                Destroy(effect, 5);
            }
            else if (Input.GetMouseButtonDown(2))
            {

                speed = 10f;
                cooldownTimer = SpeedUpDuration;
            }
            if (cooldownTimer <= 0)
            {

                speed = 3f;
            }
        
        }
        else if(flag)
        {
            isdead = true;
            animator.SetTrigger("IsDead");
            flag = false;
            

        }

        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("scene3");
        }
      
    }



    private void Move()
    {
        Vector3 targetPoint = Camera.main.transform.forward * 1000;
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint);


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(h, 0, v);
        transform.Translate(movement * Time.deltaTime * speed);
        if (h != 0 || v != 0)
        {
            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
    }
}
