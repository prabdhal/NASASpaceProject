using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour 
{
	private Transform player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		// rotation        
		transform.LookAt(player);	

	}
}
