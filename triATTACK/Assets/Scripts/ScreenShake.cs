using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    private Transform target;
    private Vector3 initialPos;
    private bool isShaking = false;
    private float pendingShakeDuration = 0f;
    private float shakeIntensity;

    void Start ()
    {
        target = GetComponent<Transform>();
        initialPos = target.localPosition;
	}

    public void Shake(float duration, float intensity)
    {
        Debug.Log("Shake called");
        shakeIntensity = intensity;
        if (duration > 0)
        {
            pendingShakeDuration += duration;
        }
    }

    void Update()
    {
        if (pendingShakeDuration > 0 && !isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        isShaking = true;

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + pendingShakeDuration)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-1f, 1f) * shakeIntensity, Random.Range(-1f, 1f) * shakeIntensity, initialPos.z);
            target.localPosition = randomPoint;
            yield return null;
        }
        
        pendingShakeDuration = 0f;
        target.localPosition = initialPos;
        isShaking = false;
    }
}
