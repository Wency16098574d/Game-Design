using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;
 
public class Enemy : MonoBehaviour
{

//    public Sprite Head;
    public int EnemyHp = 200;
    public int currentEnemyHp;
    GameObject hero;
    NavMeshAgent agent;
    Animator anim;
    CharacterController controller;
    public int attack = 20;
    public float attackInterval = 2f;
    float timer = 0;
    Health heroHealth;
    float dis;
    float times = 10.0f;
    int Action;
    bool heroInRange;
    bool isDeath = false;
    void Awake()
    {
        currentEnemyHp = EnemyHp;
        hero = GameObject.FindGameObjectWithTag("Player");
        heroHealth = hero.GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

    }
    void Update()
    {
        dis = Vector3.Distance(this.transform.position, hero.transform.position);

        if (currentEnemyHp > 0)
        {
            Activity();
        }
        else if (!isDeath)
        {
            anim.SetTrigger("Death");
            isDeath = true;
        }
    }
    void Activity()
    {
        if (dis > 15)
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isAttack", false);
            times -= Time.deltaTime;
            if (times < 0)
            {
                Action = Random.Range(0, 5);
                times = 10.0f;
            }
            switch (Action)
            {
                case 0:
                    anim.SetBool("isWalk", false);
                    transform.Rotate(0, 8 * Time.deltaTime, 0);
                    break;
                case 1:
                    anim.SetBool("isWalk", false);
                    transform.Rotate(0, -8 * Time.deltaTime, 0);
                    break;
                case 2:
                    anim.SetBool("isWalk", true);
                    controller.Move(transform.forward * -3 * Time.deltaTime);
                    break;
                default:
                    anim.SetBool("isWalk", false);
                    break;
            }
        }
        else if (dis > 1 && dis <= 15)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isAttack", false);
            anim.SetBool("isRun", true);
            transform.LookAt(hero.transform.position);
//            transform.eulerAngles += new Vector3(0, 180, 0);
            agent.SetDestination(hero.transform.position);
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isAttack", true);
            transform.LookAt(hero.transform.position);
 //           transform.eulerAngles += new Vector3(0, 180, 0);
            timer += Time.deltaTime;
            if (timer >= attackInterval)
            {
                Attack();
                timer = 0;
            }
        }
    }
    void Attack()
    {
        int ActionAttack = Random.Range(1, 5);
        if (heroHealth.currentheroHealth > 0)
        {
            switch (ActionAttack)
            {
                case 1:
                    anim.SetInteger("AttackID", 1);
                    break;
                case 2:
                    anim.SetInteger("AttackID", 2);
                    break;
                case 3:
                    anim.SetInteger("AttackID", 3);
                    break;
                default:
                    anim.SetInteger("AttackID", 4);
                    break;
            }
            if (heroInRange)
                heroHealth.TakeDamage(attack);
        }
        else
            anim.SetInteger("AttackID", 0);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == hero)
        {
            heroInRange = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == hero)
        {
            heroInRange = false;
        }
    }
}
