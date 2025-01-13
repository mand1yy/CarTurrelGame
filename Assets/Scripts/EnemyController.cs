using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float aggroRange = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    private Transform target;
    private bool isAggro = false;
    private bool isStopped = false;
    private int currentHealth;

    [Header("UI Settings")]
    [SerializeField] private Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        AlignToGround();
    }

    void Update()
    {
        if (isStopped || target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= aggroRange)
        {
            isAggro = true;
        }
        else
        {
            isAggro = false;
        }

        if (animator != null)
        {
            animator.SetBool("isAggro", isAggro);
        }

        if (isAggro)
        {
            MoveTowardsTarget();
        }

        UpdateHealthBarPosition();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        Destroy(gameObject);
    }

    private void AlignToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    private void UpdateHealthBarPosition()
    {
        if (healthBar != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * 2f;

            healthBar.transform.position = worldPosition;

            healthBar.transform.LookAt(Camera.main.transform);
            healthBar.transform.Rotate(0, 180f, 0);
        }
    }


    public void StopEnemy()
    {
        isStopped = true;
        isAggro = false;

        if (animator != null)
        {
            animator.SetBool("isAggro", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController carController = other.GetComponent<CarController>();
            if (carController != null)
            {
                carController.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

}
