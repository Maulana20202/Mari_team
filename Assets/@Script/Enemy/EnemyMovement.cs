using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public Transform player;

    [Header("AI Settings")]
    public float stoppingRange = 3f;

    public float rotationSpeed;
    private NavMeshAgent navMeshAgent;
    

    void Start()
    {
        // Ambil komponen NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on this GameObject.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Gerakkan enemy hanya jika jaraknya lebih dari stopping range
        if (distanceToPlayer > stoppingRange)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.isStopped = true;
             // Rotasi enemy menghadap ke player (hanya sumbu X dan Z)
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0f; // Abaikan rotasi sumbu Y
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        // Rotasi musuh tetap otomatis dikelola oleh NavMeshAgent (jika diperlukan manual, disable auto-rotation)
        navMeshAgent.updateRotation = true;
    }
}
