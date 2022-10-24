using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public State enemyState = State.Moving;

    public Transform currentNavPoint;
    public NavMeshAgent agent;
    public Animator anim;

    public bool alive = true;

    public int strength = 100;
    public float health = 100;
    public int scoreOnDeath = 100;

    public float minWaitTime = 3f;
    public float maxWaitTime = 10f;

    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 2f;

    public void Start()
    {
        StartCoroutine(EnemyActionsCoroutine());
    }

    public void Update()
    {
        if (alive && enemyState != State.Attacking)
            anim.SetFloat("Speed", agent.velocity.magnitude);

        if (GameManager.Paused)
            agent.destination = transform.position;
        else if (alive && enemyState != State.Attacking)
            agent.destination = currentNavPoint.position;
    }

    public IEnumerator EnemyActionsCoroutine()
    {
        yield return new WaitUntil(() => currentNavPoint != null && agent != null);

        while (alive)
        {
            if (!GameManager.Paused)
            {
                switch (enemyState)
                {
                    case State.Moving:
                        if (agent.remainingDistance < 5.0f)
                        {
                            enemyState = State.Hiding;
                            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                        }
                        break;

                    case State.Hiding:
                        enemyState = State.Moving;
                        Move();
                        break;

                    case State.Attacking:
                        agent.destination = transform.position;
                        Attack();
                        yield return new WaitForSeconds(Random.Range(minAttackCooldown, maxAttackCooldown));
                        break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void Move()
    {
        currentNavPoint = EnemyManager.GetNextNavPoint(currentNavPoint);
        agent.destination = currentNavPoint.position;
    }

    public void Attack()
    {
        GameManager.playerHealth -= strength;
        anim.SetTrigger("Attack" + Random.Range(1, 2));
    }

    public void Damage(float amount)
    {
        health -= amount;

        if (health < 0)
            StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        GameManager.currentScore += scoreOnDeath;
        alive = false;
        agent.destination = transform.position;
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Attack Zone")
            enemyState = State.Attacking;
    }

    public enum State
    {
        Hiding,
        Moving,
        Attacking
    }
}
