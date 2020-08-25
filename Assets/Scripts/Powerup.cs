﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField] //0= Triple SHot , 1 = Speed , 2 = Shield
    private int powerupID;

    [SerializeField]
    private AudioClip _powerupAudioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);

            if(player != null)
            {
                switch(powerupID)
                { case 0:
                    player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();                        
                        break;
                    case 2:
                        player.ShieldsActive();                       
                        break;
                    case 3:
                        player.BulletsActive();
                        break;
                    case 4:
                        player.HealthBoostActive();
                        break;
                    case 5:
                        player.NegativeSpeedBoostActive();
                        break;
                    case 6:
                        player.HomingProjectileBoostActive();
                        break;
                    default:
                        break;
                

                }
            }

            Destroy(this.gameObject);
        }
    }
}
