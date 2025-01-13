using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private Transform turret;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;

    private Camera mainCamera;
    private float nextFireTime;
    private bool isFiring = false;
    private bool isGameStarted = false;
    private bool canFire = false;
    private Vector2 lastInputPosition;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isGameStarted)
        {
            AimTurret();
        }

        if (isFiring)
        {
            HandleShooting();
        }

        HandleInput();
    }

    public void StartGame()
    {
        isGameStarted = true;
        canFire = false;
        Invoke("EnableFiring", 2f);
    }

    private void EnableFiring()
    {
        canFire = true;
    }

    public void DisableTurret()
    {
        isFiring = false;
        isGameStarted = false;
        canFire = false;
    }

    private void AimTurret()
    {
        Vector2 inputPosition;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
        }
        else
        {
            return;
        }
#else
        if (Input.touchCount > 0) // Для мобільних пристроїв (тач)
        {
            inputPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }
#endif

        if (lastInputPosition == Vector2.zero)
        {
            lastInputPosition = inputPosition;
        }

        float deltaX = inputPosition.x - lastInputPosition.x;
        turret.Rotate(0, 0, deltaX * 0.1f);

        lastInputPosition = inputPosition;
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (!canFire) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && isGameStarted && canFire)
        {
            StartFiring();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopFiring();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isGameStarted && canFire)
            {
                StartFiring();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                StopFiring();
            }
        }
#endif
    }

    public void StartFiring()
    {
        isFiring = true;
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
