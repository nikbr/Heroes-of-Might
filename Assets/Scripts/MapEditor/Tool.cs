using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool {
	public Dictionary<string, int> materialToValue = new Dictionary<string, int>{
		{"Dirt",0},
		{"Grass",1},
		{"Gravel",2},
		{"Plain",3},
		{"Sand",4},
		{"Water",5}
	};
	public string type;
	public int value;
	public Tool(){
		type = "terrain";
		value = 0;
	}
}
