using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject impactEffect;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (impactEffect != null)
            {
                Instantiate(impactEffect, collision.collider.bounds.center, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
