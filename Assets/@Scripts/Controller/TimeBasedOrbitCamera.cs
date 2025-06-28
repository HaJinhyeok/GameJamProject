using UnityEngine;
using System.Collections.Generic;

public class TimeBasedOrbitCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraKeyframe
    {
        public float time;
        public bool isFirstPerson = false;
        public float height = 2f;
        public float distance = 5f;
        public Vector3 rotationOffset = Vector3.zero;
        public bool orbitMode = false;
        public float orbitSpeed = 30f; // degrees per second
    }

    public Transform target;
    public List<CameraKeyframe> keyframes = new List<CameraKeyframe>();

    [Header("Lerp Speeds")]
    public float positionLerpSpeed = 5f;
    public float rotationLerpSpeed = 5f;

    private float elapsedTime = 0f;
    private float orbitAngle = 0f;

    void Update()
    {
        if (!target || keyframes.Count == 0) return;

        elapsedTime += Time.deltaTime;

        // 현재 키프레임 쌍 찾기
        CameraKeyframe before = keyframes[0];
        CameraKeyframe after = keyframes[keyframes.Count - 1];
        for (int i = 0; i < keyframes.Count - 1; i++)
        {
            if (elapsedTime >= keyframes[i].time && elapsedTime < keyframes[i + 1].time)
            {
                before = keyframes[i];
                after = keyframes[i + 1];
                break;
            }
        }

        float segmentDuration = after.time - before.time;
        float t = segmentDuration > 0f ? Mathf.InverseLerp(before.time, after.time, elapsedTime) : 0f;

        // 보간된 파라미터 계산
        float height = Mathf.Lerp(before.height, after.height, t);
        float distance = Mathf.Lerp(before.distance, after.distance, t);
        Vector3 rotOffset = Vector3.Lerp(before.rotationOffset, after.rotationOffset, t);
        bool orbitMode = t < 0.5f ? before.orbitMode : after.orbitMode;
        float orbitSpeed = Mathf.Lerp(before.orbitSpeed, after.orbitSpeed, t);
        float isFirstPersonVal = Mathf.Lerp(before.isFirstPerson ? 1 : 0, after.isFirstPerson ? 1 : 0, t);

        // 목표 위치 계산
        Vector3 desiredPosition;

        if (isFirstPersonVal >= 0.5f)
        {
            // 1인칭 시 target 위치 바로 위
            desiredPosition = target.position + Vector3.up * height;
        }
        else if (orbitMode)
        {
            // 궤도 회전 방식
            orbitAngle += orbitSpeed * Time.deltaTime;
            if (orbitAngle >= 360f) orbitAngle -= 360f;

            float angleRad = orbitAngle * Mathf.Deg2Rad;

            Vector3 orbitDirection = new Vector3(Mathf.Sin(angleRad), 1f, -Mathf.Cos(angleRad)).normalized;
            orbitDirection = target.TransformDirection(orbitDirection);

            Vector3 baseBackOffset = -target.forward * 5f;
            desiredPosition = target.position + baseBackOffset + orbitDirection * distance;
        }
        else
        {
            // 일반 3인칭
            Vector3 backOffset = -target.forward * distance;
            desiredPosition = target.position + Vector3.up * height + backOffset;
        }

        // 위치 보간
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * positionLerpSpeed);

        // 회전 처리
        Vector3 lookTarget = target.position + target.forward * 5f;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        Quaternion finalRotation = targetRotation * Quaternion.Euler(rotOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotationLerpSpeed);
    }
}
