using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSphere : MonoBehaviour
{
    private Transform _enemy;

    private float _speed = 4.0f;

    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        if(_enemy == null)
        {
            Debug.LogError("Enemy is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        Destroy(this.gameObject, 0.5f);
    }

    void CalculateMovement()
    {        
        if (_enemy != null)
        {
            Vector3 direction = _enemy.position - transform.position;
            direction.Normalize();

            transform.localRotation = Quaternion.LookRotation(direction);

            _velocity = direction * _speed;

            transform.Translate(_velocity * Time.deltaTime);
        }
    }
}
