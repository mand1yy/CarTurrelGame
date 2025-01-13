using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private Transform sideViewPosition;
    [SerializeField] private Transform backViewPosition; 
    [SerializeField] private CarController carController;
    [SerializeField] private TurretController turretController;

    [Header("Settings")]
    [SerializeField] private float cameraTransitionDuration = 1f;

    private bool gameStarted = false;

    void Start()
    {
        mainCamera.transform.position = sideViewPosition.position;
        mainCamera.transform.rotation = sideViewPosition.rotation;

        carController.StopMoving();
        turretController.DisableTurret();
    }

    void Update()
    {
        if (!gameStarted && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)))
        {
            gameStarted = true;
            StartCoroutine(StartGameSequence());
        }
    }

    private IEnumerator StartGameSequence()
    {
        turretController.StartGame();

        yield return StartCoroutine(MoveCameraToPosition(backViewPosition));

        carController.StartMoving();
    }

    private IEnumerator MoveCameraToPosition(Transform targetPosition)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        float elapsedTime = 0f;
        while (elapsedTime < cameraTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraTransitionDuration;

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition.position, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetPosition.rotation, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition.position;
        mainCamera.transform.rotation = targetPosition.rotation;
    }
}
