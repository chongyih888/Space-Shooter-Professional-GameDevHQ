using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Transform camTransform;

    [SerializeField]
    private float shakeTime;

    [SerializeField]
    private float shakeRange;
    Vector3 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = this.transform;
        originalPosition = camTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public void Shake()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        float elapseTime = 0;

        while(elapseTime < shakeTime)
        {
            Vector3 pos = originalPosition + Random.insideUnitSphere * shakeRange;

            pos.z = originalPosition.z;

            camTransform.position = pos;

            elapseTime += Time.deltaTime;

            yield return null;
        }

        camTransform.position = originalPosition;
    }

}
