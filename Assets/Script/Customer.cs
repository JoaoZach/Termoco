using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Transform target; // balcão
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }
}