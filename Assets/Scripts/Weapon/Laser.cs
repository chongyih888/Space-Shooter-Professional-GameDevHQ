using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    [SerializeField]
    private bool _isPlayer = false;
    
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false && _isPlayer == false)
        {
            Moveup();

        }
        else if (_isEnemyLaser == true && _isPlayer == true )
        {
            Moveup();
        }
        else
        {
            MoveDown();
        }
    }

    void Moveup()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser() {
        _isEnemyLaser = true;
    }

    public void AssignEnemyLaserPlayer()
    {
        _isEnemyLaser = true;
        _isPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
