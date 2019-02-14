﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : MonoBehaviour {

	private float healAmount = 0;

	public void AddHealAmount(float amt)
	{
		healAmount += amt;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerHealth>().HealPlayer(healAmount);
		}
	}

}
