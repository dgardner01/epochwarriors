using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject player, enemy;
    public float magnitude;
    public float frequency;
    public float lensSize;
    public float zoomSpeed;
    float lerpedLensSize;
    public float y;
    public Transform target;
    public Vector3 center;
    public Vector2 shake;
    public CinemachineVirtualCamera cvc;
    private void Update()
    {
        center = (player.transform.position + enemy.transform.position)/2;
        center.y = y;
        target.position = center;
        var perlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = shake.x;
        perlin.m_FrequencyGain = shake.y;
        cvc.m_Lens.OrthographicSize = lerpedLensSize;
    }
    private void FixedUpdate()
    {
        lerpedLensSize = Mathf.Lerp(lerpedLensSize, lensSize,zoomSpeed);
        if (shake.x > 0.05f)
        {
            shake.x /= 1.1f;
        }
        if (shake.y > 0.025f)
        {
            shake.y /= 1.1f;
        }
    }
    public IEnumerator ScreenShake(float duration, float magnitude, float frequency)
    {
        lerpedLensSize = lensSize + duration;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        shake.x = magnitude;
        shake.y = frequency;
    }
}
