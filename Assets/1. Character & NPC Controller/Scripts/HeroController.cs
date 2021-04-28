using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class HeroController : MonoBehaviour
{
    public AttackDefinition demoAttack;
    public Aoe aoeStompAttack;

    Animator animator;
    NavMeshAgent agent;
    CharacterStats stats;

    private GameObject attackTarget;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void SetDestination(Vector3 destination)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = destination;
    }

    public void DoStomp(Vector3 destination)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        StartCoroutine(GoToTargetAndStomp(destination));
    }

    private IEnumerator GoToTargetAndStomp(Vector3 destination)
    {
        while(Vector3.Distance(transform.position, destination) > aoeStompAttack.Range)
        {
            agent.destination = destination;
            yield return null;
        }
        agent.isStopped = true;
        animator.SetTrigger("Stomp");
    }

    public void AttackTarget(GameObject target)
    {
        var weapon = stats.GetCurrentWeapon();

        if(weapon != null)
        {
            StopAllCoroutines();

            agent.isStopped = false;
            attackTarget = target;
            StartCoroutine(PursueAndAttackTarget());
        }
    }

    private IEnumerator PursueAndAttackTarget()
    {
        agent.isStopped = false;
        var weapon = stats.GetCurrentWeapon();

        while(Vector3.Distance(transform.position, attackTarget.transform.position) > weapon.Range)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        transform.LookAt(attackTarget.transform);
        animator.SetTrigger("Attack");
    }

    public void Hit()
    {
        // Have our weapon attack the attack target
        if (attackTarget != null)
            stats.GetCurrentWeapon().ExecuteAttack(gameObject, attackTarget);
    }

    public void Stomp()
    {
        aoeStompAttack.Fire(gameObject, gameObject.transform.position, LayerMask.NameToLayer("PlayerSpells"));
    }
}
