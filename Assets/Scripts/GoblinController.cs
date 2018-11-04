using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GoblinController : MonoBehaviour {
    private Animator animator;
    public float runSpeed = 3;
    public float rotateSpeed = 3;
    public float lookDistance = 5;
    public float attackDistance = 1;
    public float attackDuration = 1.5f;
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
    private GameObject audio;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        selfTransform=transform;
        animator = GetComponent<Animator>();
        currentEnemyHp = EnemyHp;
        heroHealth = player.GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentrunSpeed = runSpeed;
        awake = false;
        audio = gameObject.transform.GetChild(4).gameObject;
    }
    private void Update()
    {
        cooldownTimer1 -= Time.deltaTime;
        if (heroHealth.currentheroHealth > 0 && currentEnemyHp > 0)
        {
            Chase();
            if (currentrunSpeed != runSpeed&& cooldownTimer1 <= 0)
            {
                currentrunSpeed = runSpeed;
            }
        }
        else
        {
            navMeshAgent.speed = 0;
            transform.LookAt(player.transform);
            animator.SetBool("IsRun", false);
        }

        if(Vector3.Distance(transform.position, player.transform.position) <= lookDistance)
        {
            awake = true;
        }
       
    }

    void Chase()
    {
        cooldownTimer -= Time.deltaTime;
        cooldownTimer2 -= Time.deltaTime;
        if ( cooldownTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            navMeshAgent.speed = 0;
            animator.SetBool("IsRun", false);
            cooldownTimer = attackDuration;
            animator.SetTrigger("IsAttack");
            StartCoroutine(AttackCheck());


        }
        else if (awake == false)
        {

            navMeshAgent.speed = 0;
            animator.SetBool("IsRun", false);
            transform.LookAt(player.transform);
        }

        else if (awake && cooldownTimer <= 0)
        {
            if (cooldownTimer2 <= 0)
            {
                audio.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
                cooldownTimer2 = 0.8f;
            }
           
            navMeshAgent.speed = currentrunSpeed;
            var lookRotation = Quaternion.LookRotation(player.transform.position - selfTransform.position);
            selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("IsRun", true);
        }
        

           
    }
    public void TakeDamage(int a)
    {
        awake = true;
        currentEnemyHp -= a;
        if (currentEnemyHp <= 0 && flag)
        {
            StartCoroutine(DeathSound());
            animator.SetTrigger("IsDeath");
            isDeath = true;
            flag = false;
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
    IEnumerator AttackCheck()
    {
        yield return new WaitForSeconds(0.6f);
        audio.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.2f);

            DamageInRange = Sector(0.5f);

     
        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }

    }
    IEnumerator DeathSound()
    {
        yield return new WaitForSeconds(1f);
        audio.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        audio.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();


    }
}
