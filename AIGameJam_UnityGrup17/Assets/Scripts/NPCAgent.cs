using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class NPCAgent : Agent
{
    public Transform player; // Oyuncu karakterinin transform'u
    public float detectionRadius = 20f; // NPC'nin oyuncuyu tespit etme mesafesi
    public float attackRadius = 6f; // NPC'nin saldýrý mesafesi
    public float wanderRadius = 10f; // NPC'nin rastgele dolaþma mesafesi
    public float wanderTimer = 4f; // Rastgele bir noktaya gitme süresi

    private NavMeshAgent agent;
    //private Animator animator;
    private float timer;
    private bool isAttacking;

    public override void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        timer = wanderTimer;
    }

    public override void OnEpisodeBegin()
    {
        // NPC'nin ve oyuncunun baþlangýç konumlarýný belirleyin
        transform.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        player.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // NPC'nin oyuncuya olan mesafesini gözlemleyin
        sensor.AddObservation(Vector3.Distance(player.position, transform.position));

        // NPC'nin kendi konumunu gözlemleyin
        sensor.AddObservation(transform.localPosition);

        // NPC'nin yönünü gözlemleyin
        sensor.AddObservation(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;

        // NPC'nin hareketini kontrol edin
        Vector3 move = new Vector3(continuousActions[0], 0, continuousActions[1]);
        agent.Move(move * Time.deltaTime * 10f);

        // Oyuncuya olan mesafeyi kontrol edin
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Ödül mekanizmasý
        AddReward(-0.001f); // Zamanla küçük negatif ödül

        if (distanceToPlayer <= attackRadius)
        {
            // Saldýrý davranýþý
            if (!isAttacking)
            {
                agent.SetDestination(transform.position); // Hareket etmeyi durdur
                //animator.SetTrigger("4");
                //animator.SetBool("isWalking", false);

                isAttacking = true;
                AddReward(1.0f); // Saldýrý ödülü
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // Takip davranýþý
            agent.SetDestination(player.position);
            //animator.SetBool("isWalking", true);
            //animator.ResetTrigger("4");

            isAttacking = false;
            AddReward(0.1f); // Oyuncuya yaklaþma ödülü
        }
        else
        {
            // Rastgele dolaþma davranýþý
            //animator.ResetTrigger("4");
            //animator.SetBool("isWalking", true);

            isAttacking = false;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
                AddReward(0.01f); // Rastgele dolaþma ödülü
            }
        }

        timer += Time.deltaTime;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
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

