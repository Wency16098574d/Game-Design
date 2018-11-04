using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KinghtController : MonoBehaviour {
    private Animator animator;
    public float runSpeed = 6;
    public float rotateSpeed = 3;
    public float walkDistance = 3;
    public float attackDistance = 1;
    public float attackDuration1 = 1.33f;
    public float attackDuration2 = 1.667f;
    public float attackDuration3 = 1.167f;
    public float attackDuration4 = 1.767f;
    public float speedDownDuration = 2f;
    public int EnemyHp = 200;
    public int currentEnemyHp;
    public int attack = 20;
    bool heroInRange;
    bool isDeath = false;
    private float cooldownTimer;
    private float cooldownTimer1;
    private float cooldownTimer2;
    private Transform selfTransform;
    GameObject player;
    Health heroHealth;
    bool flag = true;
    private NavMeshAgent navMeshAgent;
    private float currentrunSpeed;
    private float SkillDistance = 2;
    //扇形的角度 也就是攻击的角度
    private float SkillJiaodu = 90;
    private bool DamageInRange;
    public bool awake;
    private bool isSheild;
//    private GameObject audio;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        selfTransform = transform;
        animator = GetComponent<Animator>();
        currentEnemyHp = EnemyHp;
        heroHealth = player.GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentrunSpeed = runSpeed;
        awake = false;
        isSheild = false;
 //       audio = gameObject.transform.GetChild(4).gameObject;
    }
    private void Update()
    {
        cooldownTimer1 -= Time.deltaTime;
        if (heroHealth.currentheroHealth > 0 && currentEnemyHp > 0)
        {
            Chase();
            if (currentrunSpeed != runSpeed && cooldownTimer1 <= 0)
            {
                currentrunSpeed = runSpeed;
            }
        }
        else
        {
            navMeshAgent.speed = 0;
            transform.LookAt(player.transform);
            animator.SetBool("1handrun", false);
            animator.SetBool("2handwalk", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= walkDistance)
        {
            awake = true;
        }

    }

    void Chase()
    {

            isSheild = false;
            cooldownTimer -= Time.deltaTime;
            cooldownTimer2 -= Time.deltaTime;
            if (cooldownTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                navMeshAgent.speed = 0;
                animator.SetBool("1handrun", false);
                animator.SetBool("2handwalk", false);
                float random = Random.Range(0, 2);
            if (currentEnemyHp > EnemyHp * 0.5f)
            {
                if (random == 1)
                {
                    animator.SetTrigger("1hand2hit");
                    StartCoroutine(AttackCheck1());
                    cooldownTimer = attackDuration1;
                }
                else
                {
                    animator.SetTrigger("1hand3hit");
                    StartCoroutine(AttackCheck2());
                    cooldownTimer = attackDuration2;
                }
            }
            else
            {
                if (random == 1)
                {
                    animator.SetTrigger("2hand2hit");
                    StartCoroutine(AttackCheck1());
                    cooldownTimer = attackDuration3;
                }
                else
                {
                    animator.SetTrigger("2hand3hit");
                    StartCoroutine(AttackCheck2());
                    cooldownTimer = attackDuration4;
                }
            }
                
               


            }
            else if (awake == false)
            {
                isSheild = true;
                navMeshAgent.speed = 0;
                animator.SetBool("1handrun", false);
                animator.SetBool("2handwalk", false);
                transform.LookAt(player.transform);
            }

            else if (awake && cooldownTimer <= 0 &&Vector3.Distance(player.transform.position,transform.position)<walkDistance)
            {
                isSheild = false;
                animator.SetBool("2handwalk", false);
                if (cooldownTimer2 <= 0)
                {
                    //                audio.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
                    cooldownTimer2 = 0.8f;
                }

                navMeshAgent.speed = currentrunSpeed;
                var lookRotation = Quaternion.LookRotation(player.transform.position - selfTransform.position);
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                navMeshAgent.SetDestination(player.transform.position);
                animator.SetBool("1handrun", true);
            }
            else if (awake && cooldownTimer <= 0 && Vector3.Distance(player.transform.position, transform.position) > walkDistance)
            {
                isSheild = true;
                animator.SetBool("1handrun", false);
                navMeshAgent.speed = currentrunSpeed*0.7f;
                var lookRotation = Quaternion.LookRotation(player.transform.position - selfTransform.position);
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                navMeshAgent.SetDestination(player.transform.position);
                animator.SetBool("2handwalk", true);
            }
        
        
        



    }
    public void TakeDamage(int a)
    {
        awake = true;
        if (isSheild)
        {
            animator.SetTrigger("SheildBlock");
        }
        else
        {
            currentEnemyHp -= a;
        }
       
        if (currentEnemyHp <= 0 && flag)
        {
            StartCoroutine(DeathSound());
            animator.SetTrigger("IsDeath");
            isDeath = true;
            flag = false;
            transform.position = transform.position;
            transform.GetComponent<Rigidbody>().freezeRotation = true;
            Destroy(gameObject, 4f);
        }
    }
    public void SpeedDown()
    {
        cooldownTimer1 = speedDownDuration;
        currentrunSpeed *= 0.5f;
    }
    public bool Sector(float range)
    {
        bool result = false;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //玩家正前方的向量
        Vector3 norVec = transform.rotation * Vector3.forward;
        //玩家与敌人的方向向量
        Vector3 temVec = player.transform.position - transform.position;
        //求两个向量的夹角
        float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;
        if (distance < SkillDistance)
        {
            if (jiajiao <= SkillJiaodu * range)
            {
                result = true;

            }
            else
            {
                result = false;

            }
        }
        return result;
    }
    public bool Rectangle(float range)
    {
        bool result = false;
        //计算玩家与敌人的距离
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //玩家与敌人的方向向量
        Vector3 temVec = player.transform.position - transform.position;
        //与玩家正前方做点积
        float forwardDistance = Vector3.Dot(temVec, transform.forward.normalized);
        if (forwardDistance > 0 && forwardDistance <= 10)
        {
            float rightDistance = Vector3.Dot(temVec, transform.right.normalized);

            if (Mathf.Abs(rightDistance) <= range)
            {
                result = true;
            }

        }
        return result;
    }
    IEnumerator AttackCheck1()
    {
        yield return new WaitForSeconds(0.5f);

        DamageInRange = Sector(0.5f);


        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
        //        audio.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);

        DamageInRange = Sector(0.4f);


        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }

    }
    IEnumerator AttackCheck2()
    {
        yield return new WaitForSeconds(0.5f);
        DamageInRange = Sector(0.5f);


        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
        //        audio.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);

        DamageInRange = Sector(0.5f);


        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
        yield return new WaitForSeconds(0.5f);

        DamageInRange = Rectangle(0.5f);


        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }

    }
    IEnumerator DeathSound()
    {
        yield return new WaitForSeconds(1f);
 //       audio.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
 //       audio.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();


    }
}
