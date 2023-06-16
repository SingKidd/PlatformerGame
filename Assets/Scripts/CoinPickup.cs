using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSound;
    [SerializeField] int pointsForCoinPickUp = 100;

    bool isCollected = false;

    
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !isCollected)
        {
            isCollected = true;
            AudioSource.PlayClipAtPoint(coinPickUpSound, Camera.main.transform.position);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddScore(pointsForCoinPickUp);
        }    
    }


}
