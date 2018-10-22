﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallScript : MonoBehaviour {

    private RespawnManager respawnManagerInfo;

    private void Start()
    {
        respawnManagerInfo = GameObject.Find("RespawnManager").GetComponent<RespawnManager>();
        InvokeRepeating("DeathSpin",0, .001f);
    }

    private void DeathSpin()
    {
        transform.Rotate(0, 0, 1);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision Happened!");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Fire Wall Collision Happened!");
            respawnManagerInfo.KillPlayer();
        }
       
    }
}
