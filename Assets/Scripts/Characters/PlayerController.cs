using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private CharacterStats characterStats;


    private GameObject attackTarget;
    private float lastAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        characterStats.MaxHealth = 2;
    }


    private void Update()
    {
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);

        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        if (lastAttackTime < 0)
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown; //CD
        }
    }

    //Animation Event
    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamge(characterStats, targetStats);
    }
}