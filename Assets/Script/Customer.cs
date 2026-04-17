using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("References")]
    public Transform bartender;
    public Transform[] wanderPoints;

    private NavMeshAgent agent;

    // ---------------- STATES ----------------
    private enum State
    {
        Wander,
        Decide,
        GoToBartender,
        Waiting
    }

    private State state;

    // ---------------- ORDER SYSTEM ----------------
    public enum DrinkType
    {
        Beer,
        Wine,
        Cocktail
    }

    private DrinkType currentOrder;
    private bool hasOrdered = false;

    // ---------------- UI ----------------
    [Header("UI")]
    public Image orderIcon;

    public Sprite beerSprite;
    public Sprite wineSprite;
    public Sprite cocktailSprite;

    // ---------------- TIMERS ----------------
    private float waitTimer;
    private float decisionDelay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        state = State.Wander;
        GoToRandomPoint();

        if (orderIcon != null)
            orderIcon.gameObject.SetActive(false);
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

    // ---------------- WANDER ----------------
    void HandleWander()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
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

    // ---------------- DECIDE ----------------
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

    // ---------------- BARTENDER ----------------
    void GoToBartender()
    {
        if (bartender == null) return;

        state = State.GoToBartender;
        agent.SetDestination(bartender.position);
    }

    void HandleGoToBartender()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            state = State.Waiting;
            waitTimer = 10f;

            CreateOrder();
        }
    }

    // ---------------- ORDER ----------------
    void CreateOrder()
    {
        currentOrder = (DrinkType)Random.Range(0, 3);
        hasOrdered = true;

        UpdateOrderUI();

        Debug.Log("Customer wants: " + currentOrder);
    }

    void UpdateOrderUI()
    {
        if (orderIcon == null) return;

        orderIcon.gameObject.SetActive(true);

        switch (currentOrder)
        {
            case DrinkType.Beer:
                orderIcon.sprite = beerSprite;
                break;

            case DrinkType.Wine:
                orderIcon.sprite = wineSprite;
                break;

            case DrinkType.Cocktail:
                orderIcon.sprite = cocktailSprite;
                break;
        }
    }

    void HideOrderUI()
    {
        if (orderIcon != null)
            orderIcon.gameObject.SetActive(false);
    }

    // ---------------- WAITING ----------------
    void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            LeaveAngry();
        }
    }

    // ---------------- PLAYER INTERACTION ----------------
    void OnMouseDown()
    {
        if (state == State.Waiting && hasOrdered)
        {
            TryServeFromInventory();
        }
    }

    void TryServeFromInventory()
    {
        bool success = Inventory.Instance.UseItem((Inventory.ItemType)currentOrder);

        if (success)
        {
            Debug.Log("Correct drink served!");
            LeaveHappy();
        }
        else
        {
            Debug.Log("You don't have the item!");
        }
    }

    // ---------------- RESULTS ----------------
    void LeaveHappy()
    {
        hasOrdered = false;
        HideOrderUI();

        state = State.Wander;
        GoToRandomPoint();
    }

    void LeaveAngry()
    {
        hasOrdered = false;
        HideOrderUI();

        state = State.Wander;
        GoToRandomPoint();
    }
}