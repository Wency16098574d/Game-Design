using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour {

    public float runSpeed = 3;
    public float attackDistance = 4;
    public float attackDuration = 2.167f;
    public float attackDuration1 = 3.5f;
    public float dazhaoDuration = 2f;
    public float dazhao2Duration =7.5f;
    public float yuanchengDuration = 3f;
    public float speedDownDuration = 2f;
    public float rotateSpeed = 3;
    public bool awake = true;
    public bool state1 = false;
    public bool state1to2 = false;
    public bool state2 = false;
    public bool state2to3 = false;
    public bool state3 = false;
    public bool isDead = false;
    public float roarDuration=5f;
    public Transform flyTarget;
    public GameObject dazhao;
    public GameObject state2to3effect;
    public GameObject dazhao2;
    public GameObject state3attack;
    public Transform AttachPoint;
    private bool roar1 = true;
    private bool roar2;
    private bool stand;
    private bool stand1;
    public float currentrunSpeed;
    private bool heroInRange;
    private float cooldownTimer;
    private float cooldownTimer1;
    private float cooldownTimer2;
    private float cooldownTimer3;
    private Health heroHealth;
    private NavMeshAgent navMeshAgent;
    private GameObject Player;
    private Animator animator;
    private GameObject BossPosition;
    private float SkillDistance = 5;
    //扇形的角度 也就是攻击的角度
    private float SkillJiaodu = 90;
    private bool DamageInRange;
    private int attackcount = 0;
    private GameObject audio;
    private bool flag;
    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        BossPosition= GameObject.Find("BossPosition");
        heroHealth = Player.GetComponent<Health>();
        awake = false;
        state1 = false;
        state1to2 = false;
        state2 = false;
        state2to3 = false;
        state3 = false;
        isDead = false;
        roar1 = true;
        roar2 = true;
        stand = true;
        stand1 = true;
        flag = true;
        currentrunSpeed = runSpeed;
        audio = gameObject.transform.GetChild(6).gameObject;
}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(BossPosition.transform.position, Player.transform.position) < 20&&flag)
        {
            awake = true;
            flag = false;
        }


        if (awake && isDead==false)
        {
            if (state1)
            {
                cooldownTimer -= Time.deltaTime;
                heroInRange = Sector(0.5f);
                if (cooldownTimer <= 0 && Vector3.Distance(BossPosition.transform.position, Player.transform.position) <= attackDistance && heroInRange == true)
                {
                    attackcount++;
                    if (attackcount % 4 != 0)
                    {
                        cooldownTimer = attackDuration;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("2HitCombo");
                        StartCoroutine(AttackCheck());
                    }
                    else
                    {
                        cooldownTimer = attackDuration1;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("3HitCombo");
                        StartCoroutine(AttackCheck1());
                    }

                    //计算玩家与敌人的距离


                }
                else if (cooldownTimer <= 0)
                {
                    navMeshAgent.speed = currentrunSpeed;
                    navMeshAgent.SetDestination(Player.transform.position);
                    animator.SetBool("IsRun", true);
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                    //           transform.LookAt(Player.transform.position);
                }
            }
            else if (state1to2)
            {
                cooldownTimer1 -= Time.deltaTime;
                cooldownTimer2 -= Time.deltaTime;

                if (roar1 && cooldownTimer1 <= 0)
                {
                    animator.SetBool("IsRun", false);
                    navMeshAgent.speed = 0;
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                    cooldownTimer1 = roarDuration;
                    animator.SetTrigger("GetHit");
                    StartCoroutine(Hitroarsound());
                    roar1 = false;
                }
                else if(cooldownTimer1 <= 0)
                {
                    navMeshAgent.enabled=false;
                    transform.position = Vector3.MoveTowards(transform.position, flyTarget.position, 5 * Time.deltaTime);
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                    animator.SetBool("Fly", true);
                    audio.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
                    if (cooldownTimer2 <= 0)
                    {
                        cooldownTimer2 = dazhaoDuration;
                        StartCoroutine(Dazhao());
                    }


                    
                }
            }
            else if (state2)
            {
                if (stand)
                {
                    if (transform.position.y != 0)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 5 * Time.deltaTime);
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                        animator.SetBool("Fly", true);
                    }
                    else if (stand)
                    {
                        audio.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Stop();
                        animator.SetBool("Fly", false);
                        navMeshAgent.enabled = true;
                        navMeshAgent.Warp(transform.position);
                        stand = false;

                    }
                }
                

                cooldownTimer -= Time.deltaTime;
                heroInRange = Sector(0.5f);
                if (cooldownTimer <= 0 && Vector3.Distance(BossPosition.transform.position, Player.transform.position) <= attackDistance && heroInRange == true)
                {
                    attackcount++;
                    if (attackcount % 4 != 0)
                    {
                        cooldownTimer = attackDuration;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("2HitComboForward");
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                        StartCoroutine(AttackCheck());
                    }
                    else
                    {
                        cooldownTimer = attackDuration1;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("3HitComboForward");
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                        StartCoroutine(AttackCheck1());

                    }

                    //计算玩家与敌人的距离

                }
                else if (cooldownTimer <= 0)
                {
                    navMeshAgent.speed = currentrunSpeed;
                    navMeshAgent.SetDestination(Player.transform.position);
                    animator.SetBool("IsRun", true);
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                    //           transform.LookAt(Player.transform.position);
                }

            }
            else if (state2to3)
            {
                cooldownTimer1 -= Time.deltaTime;
                cooldownTimer2 -= Time.deltaTime;

                if (roar2 && cooldownTimer1 <= 0)
                {
                    animator.SetBool("IsRun", false);
                    navMeshAgent.speed = 0;
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                    cooldownTimer1 = roarDuration;
                    animator.SetTrigger("GetHit2");
                    StartCoroutine(Hitroarsound2());
                    StartCoroutine(s2to3effect());
                    roar2 = false;
                }
                else if (cooldownTimer1 <= 0)
                {
                    navMeshAgent.enabled = false;
                    transform.position = Vector3.MoveTowards(transform.position, flyTarget.position, 5 * Time.deltaTime);
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                    animator.SetBool("Fly", true);
                    audio.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
                    if (cooldownTimer2 <= 0)
                    {
                        cooldownTimer2 = dazhao2Duration;
                        StartCoroutine(Dazhao());
                    }

                }
            }
            else if (state3)
            {
                if (stand1)
                {
                    if (transform.position.y != 0)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 5 * Time.deltaTime);
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                        animator.SetBool("Fly", true);
                    }
                    else if (stand1)
                    {
                        audio.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Stop();
                        animator.SetBool("Fly", false);
                        navMeshAgent.enabled = true;
                        navMeshAgent.Warp(transform.position);
                        stand1 = false;

                    }
                }
                cooldownTimer -= Time.deltaTime;
                heroInRange = Sector(0.5f);
                runSpeed = 6;
                navMeshAgent.speed = currentrunSpeed;
                if (cooldownTimer <= 0 && Vector3.Distance(BossPosition.transform.position, Player.transform.position) <= attackDistance && heroInRange == true)
                {
                    attackcount++;
                    if (attackcount % 4 != 0)
                    {
                        cooldownTimer = attackDuration;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("2HitComboForward");
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                        StartCoroutine(AttackCheck());
                    }
                    else
                    {
                        cooldownTimer = attackDuration1;
                        navMeshAgent.speed = 0;
                        animator.SetBool("IsRun", false);
                        animator.SetTrigger("3HitComboForward");
                        var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                        StartCoroutine(AttackCheck1());
                        StartCoroutine(Dazhao());

                    }

                    //计算玩家与敌人的距离

                }
                else if (cooldownTimer <= 0)
                {
                    navMeshAgent.speed = currentrunSpeed;
                    navMeshAgent.SetDestination(Player.transform.position);
                    animator.SetBool("IsRun", true);
                    var lookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                    //           transform.LookAt(Player.transform.position);
                }
                /*
                                cooldownTimer1 -= Time.deltaTime;
                                if (cooldownTimer1 <= 0)
                                {
                                    Vector3 targetPoint = Player.transform.position;
                                    GameObject effect = Instantiate(state3attack, new Vector3(-6,0,-6)+flyTarget.transform.position,transform.rotation) as GameObject;
                                    GameObject effect1 = Instantiate(state3attack, new Vector3(6, 0,6) + flyTarget.transform.position, transform.rotation) as GameObject;
                                    effect.transform.LookAt(effect.transform.position * 2 - Player.transform.position);
                                    effect1.transform.LookAt(effect1.transform.position * 2 - Player.transform.position);
                                    cooldownTimer1 = yuanchengDuration;
                                    Destroy(effect, 2.8f);
                                    Destroy(effect1, 2.8f);
                                }
                  */
                
            }
            cooldownTimer3 -= Time.deltaTime;
            if (currentrunSpeed != runSpeed && cooldownTimer3 <= 0)
            {
                currentrunSpeed = runSpeed;
            }


        }
        else if(awake&& isDead)
        {
            animator.SetBool("IsRun", false);
            navMeshAgent.speed = 0;
            navMeshAgent.enabled = false;
            animator.SetTrigger("Death");
 //           GetComponent<CapsuleCollider>().enabled = false;
            transform.position = transform.position;
            transform.GetComponent<Rigidbody>().freezeRotation = true;
            awake = false;
            audio.transform.GetChild(6).gameObject.GetComponent<AudioSource>().Play();
        }
        


    }
    public void SpeedDown()
    {
        cooldownTimer3 = speedDownDuration;
        currentrunSpeed =runSpeed*0.5f;
    }
    public bool Sector(float range)
    {
        bool result=false;
        float distance = Vector3.Distance(BossPosition.transform.position, Player.transform.position);
        //玩家正前方的向量
        Vector3 norVec = BossPosition.transform.rotation * Vector3.forward;
        //玩家与敌人的方向向量
        Vector3 temVec = Player.transform.position - BossPosition.transform.position;
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
        float distance = Vector3.Distance(BossPosition.transform.position, Player.transform.position);
        //玩家与敌人的方向向量
        Vector3 temVec = Player.transform.position - BossPosition.transform.position;
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



    IEnumerator AttackCheck()
    {
       
        yield return new WaitForSeconds(0.5f);
        if (state1)
        {
            DamageInRange = Sector(0.5f);
            audio.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
        }
        else if (state2 || state3)
        {
            DamageInRange = Sector(0.7f);
            audio.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
        }
      

        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }

        yield return new WaitForSeconds(0.9f);
        if (state1 || state2 || state3)
        {
            audio.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
        }
       
        yield return new WaitForSeconds(0.3f);
        DamageInRange = Rectangle(1.15f);
       
        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
 //       audio.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();


    }

    IEnumerator AttackCheck1()
    {

        yield return new WaitForSeconds(0.5f);
        if (state1)
        {
            DamageInRange = Sector(0.5f);
        }
        else if (state2 || state3)
        {
            DamageInRange = Sector(0.7f);
        }
        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
        if (state1 || state2 || state3)
        {
            audio.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(0.9f);
        if (state1 || state2 || state3)
        {
            audio.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(0.3f);
        DamageInRange = Rectangle(1.15f);

        if (DamageInRange)
        {
            heroHealth.TakeDamage(5);
        }
      
        yield return new WaitForSeconds(1f);


        DamageInRange = Rectangle(9f);

        if (DamageInRange)
        {
            heroHealth.TakeDamage(10);
        }
        if (state1 || state2 || state3)
        {
            audio.transform.GetChild(8).gameObject.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator Dazhao()
    {
        if (state1to2)
        {
            for (int i = 0; i < 5; i++)
            {

                GameObject effect = Instantiate(dazhao,
               new Vector3(flyTarget.position.x, 0, flyTarget.position.z) + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), flyTarget.rotation) as GameObject;
                yield return new WaitForSeconds(0.4f);

            }
        }
            
        else if (state2to3)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject effect = Instantiate(dazhao2,
           new Vector3(flyTarget.position.x, 0, flyTarget.position.z) + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)), flyTarget.rotation) as GameObject;
                yield return new WaitForSeconds(1f);
            }
                
        }
        else if (state3)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject effect = Instantiate(dazhao2,
           new Vector3(flyTarget.position.x, 0, flyTarget.position.z) + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)), flyTarget.rotation) as GameObject;
                yield return new WaitForSeconds(1f);
            }
        }


    }

    IEnumerator s2to3effect()
    {
        yield return new WaitForSeconds(2f);
        GameObject effect = Instantiate(state2to3effect, new Vector3(BossPosition.transform.position.x+1,0, BossPosition.transform.position.z),BossPosition.transform.rotation) as GameObject;
        Destroy(effect, 5);

    }
    IEnumerator Hitroarsound()
    {
        audio.transform.GetChild(5).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        audio.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();

    }
    IEnumerator Hitroarsound2()
    {
        audio.transform.GetChild(5).gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        audio.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();

    }
}
