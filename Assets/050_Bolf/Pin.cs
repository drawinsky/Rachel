using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pin : MonoBehaviour
{
    public bool _done;

    private void OnCollisionEnter(Collision collision)
    {
        if (!_done)
        {
            if (collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin"))
            {
                //var point = GameObject.FindGameObjectWithTag("Ball").GetComponent<BowlingBallController>()._score;
                //point += 1;
                _done = true;
                
            }
        }
    }
}
