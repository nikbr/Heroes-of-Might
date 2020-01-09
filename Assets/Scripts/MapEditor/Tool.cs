using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool {
	public Dictionary<string, int> materialToValue = new Dictionary<string, int>{
		{"Grass",0},
		{"Water",1}
	};
	public string type;
	public int value;
	public Tool(){
		type = "terrain";
		value = 0;
	}
}
