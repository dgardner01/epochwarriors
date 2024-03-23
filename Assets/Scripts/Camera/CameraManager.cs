using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player, enemy;
    public Vector3 center;
    public Vector3 screenShakeOffset;
    public float shakeTimer;
    public float shakeAmount;
    public float decreaseFactor;
    private void Update()
    {
        center = (player.transform.position + enemy.transform.position) / 2;
        center.y = -268.9f;
        center.z = -10;
        transform.position = center + screenShakeOffset;
    }
    private void FixedUpdate()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 4.6f, 0.1f);
        if (shakeTimer > 0)
        {
            screenShakeOffset = Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            screenShakeOffset = Vector3.zero;
            shakeTimer = 0;
        }
    }
    public void ScreenShake(float time, float amount, float decreaseFactor)
    {
        StartCoroutine(FreezeFrame(time/2, amount));
        shakeAmount = amount;
        this.decreaseFactor = decreaseFactor;
    }

    public IEnumerator FreezeFrame(float time, float amount)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        Camera.main.orthographicSize = 4.6f - (amount / 2);
        shakeTimer = time;
    }
}
