using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    //ThirdPlayerMove Move
    float h;
    float v;
    public float speed = 6;
    public float turnSpeed = 15;
    private Transform camTransform;
    Vector3 movement;

    Vector3 camForward;
    //ThirdPlayerMove Move End

    private NavMeshAgent agent;
    private Animator anim;

    private CharacterStats characterStats;

    private GameObject attackTarget;
    private float lastAttackTime;

    private void Awake()
    {
        camTransform = Camera.main.transform;
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
        Move();
        Attack();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    #region ThirdPlayerMove Move

    void Move()
    {
        agent.isStopped = true;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //GetComponent<Rigidbody>().MovePosition(transform.position + camTransform.right * h + camForward * v);  
        transform.Translate(camTransform.right * h * speed * Time.deltaTime + camForward * v * speed * Time.deltaTime, Space.World);
        if (h != 0 || v != 0)
        {
            Rotating(h, v);
        }
    }

    void Rotating(float hh, float vv)
    {
        camForward = Vector3.Cross(camTransform.right, Vector3.up);

        Vector3 targetDir = camTransform.right * hh + camForward * vv;

        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Attack()
    {
        agent.isStopped = false;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
        }
    }

    #endregion


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