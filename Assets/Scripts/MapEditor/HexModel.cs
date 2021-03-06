﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HexModel {
	
	public HexModel(int q, int r){
		Q = q;
		R = r;
		S = -(q+r);
		type = "Grass";
		altitude = 0;
		elevationType = new string[]{"flat", "flat", "flat","flat", "flat", "flat"};
		neighborType = new string[]{"normal","normal","normal","normal","normal","normal"};
	}

	public readonly int Q;
	public readonly int R;
	public readonly int S;

	public int altitude;
	public string type;
	public string[] elevationType; // cliff, hill, mountain, flat 
	public string[] neighborType; // coast, normal, mountainside
	readonly static float RADIUS = 1f;
	readonly static float HEIGHT = RADIUS*2;
	readonly static float WIDTH = HEIGHT * Mathf.Sqrt(3)/2;
	public Vector3 Position(){
		float horizontalSpacing = WIDTH;
		float verticalSpacing = HEIGHT*0.75f;
		return new Vector3(horizontalSpacing*(Q + R/2f),0,verticalSpacing*R);
	}
}
