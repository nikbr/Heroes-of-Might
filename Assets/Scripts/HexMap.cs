using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		defaultMap();
	}
	public GameObject HexPrefab;
	// Update is called once per frame
	public void defaultMap(){
		for(int col=0;col<10;col++){
			for(int row =0;row<10;row++){
				Hex hex = new Hex(col, row);
				Instantiate(HexPrefab, hex.Position(), Quaternion.identity, this.transform);
			}
		}
	}
}