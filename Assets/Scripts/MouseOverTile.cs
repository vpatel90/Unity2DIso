﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseOverTile : MonoBehaviour {

	private Renderer rend;
	private Transform trans;
	private GameObject border;
	private bool pathSet = false;
	private List<Tile> openTiles;
	private List<Tile> closedTiles;

	
	// Use this for initialization
	void Start () {
		// not sure if we need this
		rend = GetComponent<Renderer> ();

	}

	void OnMouseEnter(){
		trans = GetComponent<Transform> ();

		border = GameObject.Find ("border");

		border.GetComponent<Transform>().position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 0.1f);


		// We only want to show the path on mouse hover. And only once. So reset this bool when mouse exits.
		if (!pathSet) {
			// Get the player's tile so we can calculate shortest path to mouse tile.
			var pTile = GameObject.Find ("AnimatedSprite").GetComponent<Tile>();
			var mTile = GetComponent<Tile>();
			Debug.Log ("Player is on tile " + pTile.x + ", " + pTile.y);
			Debug.Log ("Mouse is on tile " + mTile.x + ", " + mTile.y);
			List<Tile> adjTiles = GetAdjacentTiles(pTile);

			foreach (Tile t in adjTiles) {
				var gObj = t.GetComponent<Transform>();
				//storing the instantiate object as GameObject in clonedBorder and giving it a unique tag
				var clonedBorder = Instantiate (border, new Vector3(gObj.position.x, gObj.position.y, gObj.position.z - 0.1f), Quaternion.identity) as GameObject;
				clonedBorder.tag = "clone";
				clonedBorder.GetComponent<Renderer>().enabled = true;
			}
			pathSet = true;
		}
		border.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit(){
		removeIndicators();
		pathSet = false;
	}
	
	void OnMouseDown() {
		var tile = GetComponent<Tile>();
		if (tile != null) {
			GameManager.Instance.setTileClicked (gameObject);
			Debug.Log ("Clicked tile in position " + tile.x + ", " + tile.y);
			removeIndicators();
		}
	}
	
	void removeIndicators() {
		//creating an array of all the cloned borders by finding the unique tag
		GameObject[] borders = GameObject.FindGameObjectsWithTag("clone");
		//disabling render and destroying them (not sure why I have to disable render but it doesnt work otherwise)
		foreach (GameObject b in borders) {
			b.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
			b.GetComponent<Renderer>().enabled = false;
			Destroy (b);
		}
		//main border object gets repositioned and rendering turned off
		border.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
		border.GetComponent<Renderer>().enabled = false;
	}
	
	List<Tile> GetAdjacentTiles(Tile t) {
		List<Tile> adjTiles = new List<Tile>();
		
		Tile top = GameManager.Instance.getTileAt(t.x, t.y + 1);
		Tile bot = GameManager.Instance.getTileAt(t.x, t.y - 1);
		Tile left = GameManager.Instance.getTileAt(t.x + 1, t.y);
		Tile right = GameManager.Instance.getTileAt(t.x - 1, t.y);
		
		if (top != null) {
			Debug.Log ("Found adjacent tile at " + top.x + ", " + top.y);
			adjTiles.Add(top);
		}
		
		if (bot != null) {
			Debug.Log ("Foud adjacent tile at " + bot.x + ", " + bot.y);
			adjTiles.Add (bot);
		}
		
		if (left != null) {
			Debug.Log ("Found adjacent tile at " + left.x + ", " + left.y);
			adjTiles.Add (left);
		}
		
		if (right != null) {
			Debug.Log ("Found adjacent tile at " + right.x + ", " + right.y);
			adjTiles.Add(right);
		}
		
		return adjTiles;
	}

	// Update is called once per frame
	void Update () {

	}
}
