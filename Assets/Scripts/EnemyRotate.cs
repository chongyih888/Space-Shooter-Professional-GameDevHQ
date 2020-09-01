using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate : MonoBehaviour
{
    [SerializeField]
    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * speed);
    }
}
