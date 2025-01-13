using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [Header("Road Settings")]
    [SerializeField] private GameObject roadSegmentPrefab;
    [SerializeField] private GameObject roadTriggerPrefab;
    [SerializeField] private int additionalSegments = 3;
    [SerializeField] private float segmentLength = 10f;

    [Header("Initial Road Segments")]
    [SerializeField] private Transform initialSegment1;
    [SerializeField] private Transform initialSegment2;

    private Transform lastSegment;

    void Start()
    {
        if (initialSegment1 == null || initialSegment2 == null)
        {
            Debug.LogError("Initial road segments are not assigned in the Inspector!");
            return;
        }
        lastSegment = initialSegment2;

        GenerateAdditionalSegments();

        AddTriggerSegment();
    }

    private void GenerateAdditionalSegments()
    {
        for (int i = 0; i < additionalSegments; i++)
        {
            AddRoadSegment();
        }
    }

    private void AddRoadSegment()
    {
        Vector3 position = lastSegment.position + new Vector3(0, 0, -segmentLength);
        Quaternion rotation = Quaternion.Euler(-90, 0, 0); // Поворот сегмента дороги
        GameObject segment = Instantiate(roadSegmentPrefab, position, rotation);
        segment.transform.parent = this.transform;
        lastSegment = segment.transform;
    }

    private void AddTriggerSegment()
    {
        Vector3 position = lastSegment.position + new Vector3(0, 0, -segmentLength);
        Quaternion rotation = Quaternion.Euler(-90, 0, 0); // Поворот сегмента дороги
        GameObject triggerSegment = Instantiate(roadTriggerPrefab, position, rotation);
        triggerSegment.transform.parent = this.transform;
    }
}
