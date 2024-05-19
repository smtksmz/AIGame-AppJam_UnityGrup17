using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform player; // Oyuncu karakterinin transform'u
    public float detectionRadius = 20f; // NPC'nin oyuncuyu tespit etme mesafesi
    public float attackRadius = 6f; // NPC'nin saldýrý mesafesi
    public float wanderRadius = 10f; // NPC'nin rastgele dolaþma mesafesi
    public float wanderTimer = 4f; // Rastgele bir noktaya gitme süresi

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private bool isAttacking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRadius)
        {
            // Saldýrý davranýþý
            if (!isAttacking)
            {
                agent.SetDestination(transform.position); // Hareket etmeyi durdur
                animator.SetTrigger("4");
                animator.SetBool("isWalking", false);
                
                isAttacking = true;
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // Takip davranýþý
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.ResetTrigger("4");
            
            isAttacking = false;
        }
        else
        {
            // Rastgele dolaþma davranýþý
            animator.ResetTrigger("4");
            animator.SetBool("isWalking", true);

            isAttacking = false;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
