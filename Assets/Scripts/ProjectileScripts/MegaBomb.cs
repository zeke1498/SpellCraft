﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBomb : MonoBehaviour {

    public AudioSource explosionSource;
    public AudioClip explosionSound;

    private ProjectileDamage projectileDamageInfo;
    private float bombDamage;
    public float fireBallSpeed = 50;

    public GameObject bomb;
    

    private void Start()
    {
        projectileDamageInfo = gameObject.GetComponent<ProjectileDamage>();
        bombDamage = projectileDamageInfo.projectileDamage;
        transform.Rotate(new Vector3(0, 0, 90));
        Invoke("Explode", 2);
    }
    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * fireBallSpeed);
    }

    // transform.localScale += new Vector3(1,0);

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<PlayerHealth>().DamagePlayer(bombDamage);
            // GameObject.Find("Player").GetComponent<PlayerHealth>().playerHealthBar.fillAmount -= .25f;

        }
        else if (col.gameObject.tag == "Simulacrum")
        {
            col.gameObject.GetComponent<SimulacrumAbilities>().AbsorbDamage(bombDamage);
            Explode();
        }
        else if (col.gameObject.tag == "Absorb")
        {
            GameObject.Find("Player").GetComponent<PlayerHealth>().HealPlayer(bombDamage / 2);
            // GameObject.Find("Player").GetComponent<PlayerHealth>().playerHealthBar.fillAmount += .025f;
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "CameraTrigger" || gameObject.tag == "HealStun")
        {
            //do nothing
        }
        else if (col.gameObject.tag != "Boss" || col.gameObject.tag != "CameraTrigger" || col.gameObject.tag == "Player")
        {
            Explode();
        }
    }

    public void Explode()
    {
        explosionSource.clip = explosionSound;
        explosionSource.PlayOneShot(explosionSound);
        GameObject bomb1 = Instantiate(bomb, transform.position, transform.rotation);
        GameObject bomb2 = Instantiate(bomb, transform.position, transform.rotation);
        bomb2.transform.Rotate(0, 0, 180);
        GameObject bomb3 = Instantiate(bomb, transform.position, transform.rotation);
        bomb3.transform.Rotate(0, 0, 90);
       // GameObject bomb4 = Instantiate(bomb, transform.position, transform.rotation);
      //  bomb4.transform.Rotate(0, 0, -90);

        Destroy(gameObject);
    }
}
