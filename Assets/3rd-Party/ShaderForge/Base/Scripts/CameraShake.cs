using UnityEngine;
using System.Collections;
//作者公众号“特效基地”
//Jason 20170426 v0.1
[AddComponentMenu("Base/CameraShake")]
public class CameraShake : MonoBehaviour
{
    public AnimationCurve ShakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public float duration = 0.9f;
    public float frequency = 30;
    public float strength = 1;

    bool isPlaying;
    [HideInInspector]
    public bool canUpdate;

	void Update()
	{
		if (isPlaying) {
			isPlaying = false;
			PlayShake();
		}
	}

    void PlayShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    void OnEnable()
    {
        isPlaying = true;
        var shakes = FindObjectsOfType(typeof(CameraShake)) as CameraShake[];
        if(shakes!=null)
        foreach (var shake in shakes)
        {
            shake.canUpdate = false;
        }
        canUpdate = true;
    }

    IEnumerator Shake()
    {
        var elapsed = 0.0f;
        var camT = Camera.main.transform;
        var originalCamRotation = camT.rotation.eulerAngles;
        var direction = (transform.position - camT.position).normalized;
        var time = 0f;
        var randomStart = Random.Range(-1000.0f, 1000.0f);
        var distanceDamper = 1 - Mathf.Clamp01((camT.position - transform.position).magnitude / 100);
        Vector3 oldRotation = Vector3.zero;
        while (elapsed < duration && canUpdate) {
            elapsed += Time.deltaTime;
            var percentComplete = elapsed / duration;
            var damper = ShakeCurve.Evaluate(percentComplete) * distanceDamper;
            time += Time.deltaTime * damper;
			camT.position -= direction * Time.deltaTime * Mathf.Sin(time * frequency) * damper * strength/2;

			var alpha = randomStart + frequency * percentComplete / 10;
            var x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            var y = Mathf.PerlinNoise(1000 + alpha, alpha + 1000) * 2.0f - 1.0f;
            var z = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            if (Quaternion.Euler(originalCamRotation + oldRotation)!=camT.rotation)
                originalCamRotation = camT.rotation.eulerAngles;
			oldRotation = Mathf.Sin(time * frequency) * damper * strength * new Vector3(0.5f + y, 0.3f + x, 0.3f + z) * 2;
            camT.rotation = Quaternion.Euler(originalCamRotation + oldRotation);

            yield return null;
        }
    }
}
