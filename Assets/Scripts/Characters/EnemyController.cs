using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates
{
    GUARD, //守卫
    PATROL, //巡逻
    CHASE, //追逐
    DEAD
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;
    private Collider coll;

    protected CharacterStats characterStats;

    [Header("Basic Settings")] public float singhtRadius;
    public bool isGuard;
    private float speed;

    protected GameObject attackTarget;

    public float lookAtTime;
    private float remainlookAtTime;
    private float lastAttackTime;

    private Quaternion guardRotation;

    [Header("Patrol State")] public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;

    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;
    private bool playerDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        characterStats = GetComponent<CharacterStats>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainlookAtTime = lookAtTime;
    }

    private void Start()
    {
        characterStats.CurrentHealth = characterStats.MaxHealth;

        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD; //守卫
        }
        else
        {
            enemyStates = EnemyStates.PATROL; //巡逻
            GetNewWayPoint();
        }

        //TODO 场景切换后修改掉
        GameManager.Instance.AddObserver(this);
    }

    //TODO 切换场景时启用
    // void OnEnable()
    // {
    //     GameManager.Instance.AddObserver(this);
    // }

    void OnDisable()
    {
        if (!GameManager.IsInitialized)
        {
            return;
        }

        GameManager.Instance.RemoveObserver(this);
    }

    private void Update()
    {
        if (characterStats.CurrentHealth == 0)
        {
            isDead = true;
        }

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Death", isDead);
    }

    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD: //守卫
                isChase = false;
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                }

                break;
            case EnemyStates.PATROL: //巡逻
                isChase = false;
                agent.speed = speed * 0.5f;

                //判断是否到了随机巡逻点
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainlookAtTime > 0)
                    {
                        remainlookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }

                break;
            case EnemyStates.CHASE: //追逐
                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    //拉脱回到上一个状态
                    isFollow = false;
                    if (remainlookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainlookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                    }
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }

                //在攻击范围内则攻击
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        //暴击判断
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //执行攻击
                        Attack();
                    }
                }

                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.radius = 0;
                Destroy(gameObject, 2f);
                break;
        }
    }

    void Attack()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);
            if (TargetInAttackRange())
            {
                //近身攻击动画
                anim.SetTrigger("Attack");
            }

            if (TargetInSkillRange())
            {
                //技能攻击动画
                anim.SetTrigger("Skill");
            }
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, singhtRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else
            return false;
    }

    void GetNewWayPoint()
    {
        remainlookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, singhtRadius);
    }

    //Animation Event
    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamge(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        //获胜动画
        //停止所有一堆
        //停止Agent
        anim.SetBool("Win", true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}