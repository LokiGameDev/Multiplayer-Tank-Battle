using UnityEngine;

public class TyreTrack : MonoBehaviour
{
    // public LineRenderer line;
    // public float minDistance = 0.2f;

    // Vector3 lastPos;
    // int pointCount = 0;

    // void Start()
    // {
    //     lastPos = transform.position;
    //     line.positionCount = 1;
    //     line.SetPosition(0, lastPos);
    // }

    // void Update()
    // {
    //     float dist = Vector3.Distance(transform.position, lastPos);

    //     if (dist >= minDistance)
    //     {
    //         lastPos = transform.position;
    //         pointCount++;
    //         line.positionCount = pointCount + 1;
    //         line.SetPosition(pointCount, lastPos);
    //     }
    // }

    public LineRenderer trackPrefab;
    public float segmentDuration = 3f;
    public float minPointDistance = 0.2f;

    LineRenderer currentLine;
    float segmentTimer;
    Vector3 lastPos;

    void Start()
    {
        CreateNewSegment();
    }

    void Update()
    {
        segmentTimer += Time.deltaTime;

        if (segmentTimer >= segmentDuration)
            CreateNewSegment();

        float dist = Vector3.Distance(transform.position, lastPos);
        if (dist >= minPointDistance)
        {
            lastPos = transform.position;
            AddPoint(lastPos);
        }
    }

    void CreateNewSegment()
    {
        segmentTimer = 0f;

        currentLine = Instantiate(trackPrefab);
        currentLine.positionCount = 0;

        lastPos = transform.position;
        AddPoint(lastPos);
    }

    void AddPoint(Vector3 pos)
    {
        int count = currentLine.positionCount;
        currentLine.positionCount = count + 1;
        currentLine.SetPosition(count, pos);
    }
}
