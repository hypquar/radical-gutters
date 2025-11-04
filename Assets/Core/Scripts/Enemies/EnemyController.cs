using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;        // Точки патрулирования
    private Transform currentPoint;

    private enum State { patrol, walkToPlayer }
    [SerializeField] private State currentState = State.patrol;

    private GameObject player;
    private NavMeshAgent agent;

    [SerializeField] private float walkToPlayerSpeed = 6f;    // Скорость преследования
    [SerializeField] private float patrolSpeed = 3f;          // Скорость патрулирования

    [SerializeField] private float detectionRadius = 15f;     // Радиус звукового поиска игрока
    [SerializeField] private float attackRadius = 5f;         // Радиус немедленной атаки (game over)

    [SerializeField] private float waitTimeAtPoint = 2f;      // Время ожидания на патрульной точке
    private float waitTimer = 0f;
    private bool isWaiting = false;

    [SerializeField] private float losePlayerTime = 5f;       // Время до отказа преследования
    private float losePlayerTimer = 0f;

    [SerializeField] private float attackWarningTime = 2f; // Время на реакцию игрока
    private float attackWarningTimer = 0f;
    private bool isPlayerInAttackZone = false;

    [SerializeField] private PlayerMovement playerMovementStatus; // Предполагаемый скрипт игрока для проверки движения и корточек

    public UnityEvent OnAttack;


    private void OnDrawGizmosSelected()
    {
        // Рисуем радиус звукового поиска желтым цветом
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Рисуем радиус атаки красным цветом
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovementStatus = player.GetComponent<PlayerMovement>();                                         // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!

        agent = GetComponent<NavMeshAgent>();

        if (walkToPlayerSpeed <= 0) walkToPlayerSpeed = agent.speed;
        if (patrolSpeed <= 0) patrolSpeed = agent.speed;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
            agent.speed = patrolSpeed;
            agent.SetDestination(currentPoint.position);
        }
        else
        {
            Debug.LogWarning("Patrol points are not assigned or empty.");
        }
    }

    void Update()
    {
        if (currentState == State.patrol)
        {
            if (!isWaiting && currentPoint != null && (transform.position - currentPoint.position).magnitude < 3f)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
                agent.SetDestination(transform.position); // Остановка
            }

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    isWaiting = false;
                    WalkToNewPoint();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null || playerMovementStatus == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        bool playerIsMovingAndIsCrouching = playerMovementStatus.IsMoving && !playerMovementStatus.IsCrouching;         // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!
        bool playerIsMoving = playerMovementStatus.IsMoving;                                                    // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!

        // Проверка попадания под свет фонарика (см. ниже метод OnTriggerStay)
        // Реакция на свет реализована отдельно

        if (distanceToPlayer <= attackRadius)
        {
            if (playerIsMoving)
            {
                if (!isPlayerInAttackZone)
                {
                    isPlayerInAttackZone = true;
                    attackWarningTimer = attackWarningTime; // Запускаем таймер предупреждения
                }
                else
                {
                    attackWarningTimer -= Time.fixedDeltaTime;
                    if (attackWarningTimer <= 0f)
                    {
                        AttackPlayer(); // Игрок не отреагировал вовремя — game over
                        isPlayerInAttackZone = false;
                    }
                }
            }
            else
            {
                // Игрок неподвижен — моб исчезает, сбрасываем таймер и флаг
                DeactivateMob();
                isPlayerInAttackZone = false;
                attackWarningTimer = attackWarningTime;
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            if (playerIsMovingAndIsCrouching)
            {
                // Моб слышит движение игрока — начинает преследование
                currentState = State.walkToPlayer;
                losePlayerTimer = losePlayerTime;
                agent.speed = walkToPlayerSpeed;
                agent.SetDestination(player.transform.position);
                WalkToPlayer();
            }
            else if (currentState == State.walkToPlayer)
            {
                // Игрок не движется — перестаем преследовать
                losePlayerTimer -= Time.fixedDeltaTime;
                if (losePlayerTimer <= 0f)
                {
                    currentState = State.patrol;
                    agent.speed = patrolSpeed;
                    WalkToNewPoint();
                }
                else
                {
                    // Идет к последнему местоположению игрока
                    agent.SetDestination(player.transform.position);
                }
            }
        }
        else
        {
            // Игрок вне зон реагирования — патрулируем
            if (currentState == State.walkToPlayer)
            {
                currentState = State.patrol;
                agent.speed = patrolSpeed;
                WalkToNewPoint();
            }
        }
    }

    // Деактивация моба
    private void DeactivateMob()                                                                            // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!
    {
        Debug.Log("Монстр исчез!");
        // Тут можно реализовать переход в сцену или другую логику
    }

    // Атака игрока (game over)
    private void AttackPlayer()                                                                             // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!
    {
        Debug.LogError("Game Over: Player caught!");
        OnAttack.Invoke();
        // Тут можно реализовать переход в сцену поражения или другую логику
    }

    // Преследование игрока
    private void WalkToPlayer()                                                                             // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!
    {
        Debug.LogWarning("Тихо!");
        // Тут можно реализовать переход в сцену или другую логику
    }

    void WalkToNewPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        currentPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        currentState = State.patrol;
        agent.speed = patrolSpeed;
        agent.SetDestination(currentPoint.position);
    }

    // Обнаружение попадания света фонарика через триггер (необходимо настроить коллайдер и тег)
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flashlight"))                                                                 // !!!  ТУТ НУЖНО ИСПРАВИТЬ  !!!
        {
            DeactivateMob();
        }
    }
}
