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
    public float attackRadius = 6f; // NPC'nin sald�r� mesafesi
    public float wanderRadius = 10f; // NPC'nin rastgele dola�ma mesafesi
    public float wanderTimer = 4f; // Rastgele bir noktaya gitme s�resi

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
        // NPC'nin ve oyuncunun ba�lang�� konumlar�n� belirleyin
        transform.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        player.localPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // NPC'nin oyuncuya olan mesafesini g�zlemleyin
        sensor.AddObservation(Vector3.Distance(player.position, transform.position));

        // NPC'nin kendi konumunu g�zlemleyin
        sensor.AddObservation(transform.localPosition);

        // NPC'nin y�n�n� g�zlemleyin
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

        // �d�l mekanizmas�
        AddReward(-0.001f); // Zamanla k���k negatif �d�l

        if (distanceToPlayer <= attackRadius)
        {
            // Sald�r� davran���
            if (!isAttacking)
            {
                agent.SetDestination(transform.position); // Hareket etmeyi durdur
                //animator.SetTrigger("4");
                //animator.SetBool("isWalking", false);

                isAttacking = true;
                AddReward(1.0f); // Sald�r� �d�l�
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // Takip davran���
            agent.SetDestination(player.position);
            //animator.SetBool("isWalking", true);
            //animator.ResetTrigger("4");

            isAttacking = false;
            AddReward(0.1f); // Oyuncuya yakla�ma �d�l�
        }
        else
        {
            // Rastgele dola�ma davran���
            //animator.ResetTrigger("4");
            //animator.SetBool("isWalking", true);

            isAttacking = false;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
                AddReward(0.01f); // Rastgele dola�ma �d�l�
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

