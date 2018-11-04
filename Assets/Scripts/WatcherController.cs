using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherController : MonoBehaviour {

    public float attactDistance = 10f;
    public float moveDistacne = 5f;
    public float attackDuration = 1.5f;
    public float moveDuration = 2f;
    public GameObject Effect;
    public Transform AttachPoint;
    GameObject Player;
    private Animator animator;
    private float cooldownTimer;
    private bool awake = false;
    public float movespeed=2f;
    private Vector3 targetposition;
    public int EnemyHp = 200;
    public int currentEnemyHp;
    private bool flag = true;
    private bool isDeath = false;
    private GameObject audio;


    // Use this for initialization
    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        targetposition = Player.transform.position;
        currentEnemyHp = EnemyHp;
        audio = gameObject.transform.GetChild(3).gameObject;

    }
	// Update is called once per frame
	void Update () {
        if (currentEnemyHp>0 && Player.GetComponent<Health>().currentheroHealth>0)
        {
            cooldownTimer -= Time.deltaTime;
            if (Vector3.Distance(transform.position, Player.transform.position) > attactDistance && awake == false)
            {

            }

            if (Vector3.Distance(transform.position, Player.transform.position) < attactDistance)
            {
                if (cooldownTimer <= 0)
                {
                    cooldownTimer = attackDuration;
                    animator.SetTrigger("IsAttack");
                    audio.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
                    Vector3 targetPoint = Player.transform.position;
                    GameObject effect = Instantiate(Effect, AttachPoint.position, AttachPoint.rotation) as GameObject;
                    effect.transform.LookAt(targetPoint + Vector3.up * 2);
                    Destroy(effect, 5);
                    awake = true;
                    targetposition = transform.position + new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetposition, 3 * Time.deltaTime);
                }



            }
            else if (Vector3.Distance(transform.position, Player.transform.position) > attactDistance && awake == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z), movespeed * Time.deltaTime);
                animator.SetTrigger("IsMove");
            }


            transform.LookAt(Player.transform);
        }
        else if (currentEnemyHp <= 0)
        {
            if (flag)
            {
                audio.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
                animator.SetTrigger("IsDeath");
                isDeath = true;
                flag = false;
                Destroy(gameObject, 4);
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 3 * Time.deltaTime);

        }


    }
    public void TakeDamage(int a)
    {
        currentEnemyHp -= a;
        
    }
}
