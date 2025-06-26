using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineSpeedController : MonoBehaviour
{
    [System.Serializable]
    public class SpeedSegment
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    [System.Serializable]
    public class InputCheckpoint
    {
        //public float tValue;            // spline ���� Ÿ�̹� (0~1)
        public float tValue;
        public KeyCode requiredKey;     // �䱸�Ǵ� Ű
        //public float tolerance = 0.02f; // ��� ���� (T �� ����)
        public float tolerance = 1f;
        public SplineContainer failSpline;
    }

    public enum Axis
    {
        XAxis,
        YAxis,
        ZAxis,
        XAxisNeg,
        YAxisNeg,
        ZAxisNeg
    }
    public InputCheckpoint[] inputCheckpoints;
    private int nextInputIndex = 0;
    private bool isWaitingForInput = false;

    public SplineContainer splineContainer;
    public SpeedSegment[] speedSegments;
    public Axis forwardAxis = Axis.ZAxis;
    public Axis upAxis = Axis.YAxis;

    private float elapsedTime = 0f;
    private float totalDistance;
    private float currentT = 0f;
    private bool _isFirstArrow = true;

    private 

    void Start()
    {
        if (splineContainer == null)
            splineContainer = GetComponent<SplineContainer>();

        totalDistance = splineContainer.Splines[0].GetLength();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float currentSpeed = GetCurrentSpeed(elapsedTime);

        float deltaDistance = currentSpeed * Time.deltaTime;
        float deltaT = deltaDistance / totalDistance;
        currentT += deltaT;

        currentT = Mathf.Clamp01(currentT);

        //currentT += Time.deltaTime;

        //HandleInputCheckpoint(currentT);
        HandleInputCheckpoint(elapsedTime);

        float3 pos = splineContainer.EvaluatePosition(currentT);
        float3 tangent = splineContainer.EvaluateTangent(currentT);
        float3 up = splineContainer.EvaluateUpVector(currentT);

        Quaternion axisRemap = Quaternion.Inverse(Quaternion.LookRotation(GetAxisVector(forwardAxis), GetAxisVector(upAxis)));
        Quaternion finalRotation = Quaternion.LookRotation((Vector3)tangent, (Vector3)up);

        transform.position = (Vector3)pos;
        transform.rotation = finalRotation;
        if (currentT >= 1f)
        {
            elapsedTime = 0f;
            currentT = 0f;
        }
    }

    float GetCurrentSpeed(float time)
    {
        foreach (var segment in speedSegments)
        {
            if (time >= segment.startTime && time < segment.endTime)
                return segment.speed;
        }
        return 0f;
    }

    Vector3 GetAxisVector(Axis axis)
    {
        switch (axis)
        {
            case Axis.XAxis: return Vector3.right;
            case Axis.YAxis: return Vector3.up;
            case Axis.ZAxis: return Vector3.forward;
            case Axis.XAxisNeg: return -Vector3.right;
            case Axis.YAxisNeg: return -Vector3.up;
            case Axis.ZAxisNeg: return -Vector3.forward;
            default: return Vector3.forward;
        }
    }

    void HandleInputCheckpoint(float t)
    {
        if (nextInputIndex >= inputCheckpoints.Length) return;

        var checkpoint = inputCheckpoints[nextInputIndex];
        float minT = checkpoint.tValue - checkpoint.tolerance;
        //float maxT = checkpoint.tValue + checkpoint.tolerance;
        float maxT = checkpoint.tValue;

        if (!isWaitingForInput && t >= minT && t <= maxT)
        {
            isWaitingForInput = true;
            // �÷��̾� �Է��� ��ٸ��� ���°� ��
            Debug.Log($"�Է� �䱸: {checkpoint.requiredKey} Ű�� �����ּ���!");
            UI_Game.OnArrowActivated?.Invoke(checkpoint.requiredKey);

            if(TutorialManager.Instance.IsTutorial&&_isFirstArrow)
            {
                Time.timeScale = 0f;
                TutorialManager.Instance.StartArrowTutorial(Vector2.zero);
                _isFirstArrow = false;
                GameController.OnPreferencePanelSet?.Invoke(true);
            }
        }

        if (isWaitingForInput)
        {
            if (Input.GetKeyDown(checkpoint.requiredKey))
            {
                //UI_Game.OnArrowDeactivated?.Invoke();
                Debug.Log("�Է� ����!");
                isWaitingForInput = false;
                nextInputIndex++;
            }
            else if (t > maxT)
            {
                // ��� ���� �ѱ�
                //UI_Game.OnArrowDeactivated?.Invoke();
                //isWaitingForInput = false;
                //Debug.Log("�Է� ���� - ���� ����");
                //OnGameOver();
                if (checkpoint.failSpline == splineContainer) return;

                // �ٸ� ���ö������� �Ѿ����
                if (checkpoint.failSpline != null)
                {
                    splineContainer = checkpoint.failSpline;
                    totalDistance = splineContainer.Splines[0].GetLength();
                    currentT = 0f;
                    elapsedTime = 0f;
                    isWaitingForInput = false;

                    nextInputIndex = 0;

                    //UI_Game.OnArrowDeactivated?.Invoke();
                }
                else
                {
                    Debug.LogWarning("FailSpline�� �������� �ʾҽ��ϴ�. ���� ���� ó��.");
                    OnGameOver();
                }
            }
        }
    }

    void OnGameOver()
    {
        // �ʿ信 ���� ���߰ų� �� ��ȯ, UI ���� ��
        Debug.Log("Game Over!!!");
        Time.timeScale = 0f;
        GameManager.Instance.IsPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameController.OnCollision?.Invoke();
        OnGameOver();
    }
}

