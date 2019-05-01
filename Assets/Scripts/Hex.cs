﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex {
	
	public Hex(int q, int r){
		Q = q;
		R = r;
		S = -(q+r);
	}

	public readonly int Q;
	public readonly int R;
	public readonly int S;

	readonly static float RADIUS = 1f;
	readonly static float HEIGHT = RADIUS*2;
	readonly static float WIDTH = HEIGHT * Mathf.Sqrt(3)/2;
	public Vector3 Position(){
		float horizontalSpacing = WIDTH;
		float verticalSpacing = HEIGHT*0.75f;
		return new Vector3(horizontalSpacing*(Q + R/2f),0,verticalSpacing*R);
	}
}