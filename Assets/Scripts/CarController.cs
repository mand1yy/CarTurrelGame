using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TurretController turretController;
    [SerializeField] private Slider hpBar;
    [SerializeField] private CanvasGroup healthBarCanvasGroup;

    private Rigidbody rb;
    private bool isMoving = false;
    private int currentHealth;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        currentHealth = maxHealth;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = currentHealth;
        }

        if (healthBarCanvasGroup != null)
        {
            healthBarCanvasGroup.alpha = 0f;
        }
    }


    void FixedUpdate()
    {
        if (isMoving && !isGameOver)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        Vector3 newPosition = transform.position + -transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    public void StartMoving()
    {
        if (isGameOver) return;

        isMoving = true;

        if (healthBarCanvasGroup != null)
        {
            healthBarCanvasGroup.alpha = 1f;
        }

        if (turretController != null)
        {
            turretController.StartFiring();
        }
    }


    public void StopMoving()
    {
        isMoving = false;

        if (turretController != null)
        {
            turretController.StopFiring();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        currentHealth -= damage;

        if (hpBar != null)
        {
            hpBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }



    private void GameOver()
    {
        FindObjectOfType<GameManager>().GameOver(false);
        isGameOver = true;
        StopMoving();

        turretController.DisableTurret();

        if (healthBarCanvasGroup != null)
        {
            healthBarCanvasGroup.alpha = 0f;
        }

        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies)
        {
            enemy.StopEnemy();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Finish"))
        {
            Victory();
        }
    }

    private void Victory()
    {
        FindObjectOfType<GameManager>().GameOver(true);
        isGameOver = true;
        StopMoving();
        turretController.StopFiring();
        
    }
}
