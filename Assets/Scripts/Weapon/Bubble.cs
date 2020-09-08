using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Transform _player;

    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").transform;

        if (_player == null)
        {
            Debug.LogError("The Player is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();        
    }

    void CalculateMovement()
    {
        if (_player != null)
        {
            Vector3 direction = _player.position - transform.position;
            direction.Normalize();

            transform.localRotation = Quaternion.LookRotation(direction);

            _velocity = direction * _speed;

            transform.Translate(_velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
