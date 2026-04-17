using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [Header("References")]
    public Transform bartender;          // ponto do balcão
    public Transform[] wanderPoints;     // pontos para andar pelo bar

    private NavMeshAgent agent;

    private enum State
    {
        Wander,
        Decide,
        GoToBartender,
        Waiting
    }

    private State state;

    private float waitTimer;
    private float decisionDelay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        state = State.Wander;
        GoToRandomPoint();
    }

    void Update()
    {
        switch (state)
        {
            case State.Wander:
                HandleWander();
                break;

            case State.Decide:
                HandleDecide();
                break;

            case State.GoToBartender:
                HandleGoToBartender();
                break;

            case State.Waiting:
                HandleWaiting();
                break;
        }
    }

    // ------------------------
    // WANDER
    // ------------------------
    void HandleWander()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // chegou ao ponto → começa a decidir
            state = State.Decide;
            decisionDelay = Random.Range(2f, 5f);
        }
    }

    void GoToRandomPoint()
    {
        if (wanderPoints.Length == 0) return;

        Transform randomPoint = wanderPoints[Random.Range(0, wanderPoints.Length)];
        agent.SetDestination(randomPoint.position);
    }

    // ------------------------
    // DECIDE
    // ------------------------
    void HandleDecide()
    {
        decisionDelay -= Time.deltaTime;

        if (decisionDelay <= 0f)
        {
            float chance = Random.value;

            if (chance > 0.4f)
            {
                GoToBartender();
            }
            else
            {
                state = State.Wander;
                GoToRandomPoint();
            }
        }
    }

    // ------------------------
    // GO TO BARTENDER
    // ------------------------
    void GoToBartender()
    {
        state = State.GoToBartender;
        agent.SetDestination(bartender.position);
    }

    void HandleGoToBartender()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            state = State.Waiting;
            waitTimer = 10f; // tempo antes de ficar irritado

            Debug.Log("Customer waiting for drink");
        }
    }

    // ------------------------
    // WAITING
    // ------------------------
    void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            Debug.Log("Customer got tired and leaves");

            // volta a vaguear (ou depois podes fazer sair do bar)
            state = State.Wander;
            GoToRandomPoint();
        }
    }
}