using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public int speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Input.GetAxis ("Horizontal")*Time.deltaTime*speed,0,Input.GetAxis ("Vertical")*Time.deltaTime*speed);
		
	}
}
