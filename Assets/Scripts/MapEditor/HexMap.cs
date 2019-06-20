using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;
	
	public HexMap (EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		drawMap(context.em);
		StaticBatchingUtility.Combine(go);
	}

	public void drawMap(EditorModel em){
		
		em.hexes = new List<HexModel>();
		for (int col = 0;col<em.width;col++){
			for(int row =0;row<em.height;row++){
				em.hexes.Add(new HexModel(col, row));
			}
		}
		foreach(HexModel hmodel in em.hexes){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			mr.material = HexMaterials[0];
		}
	}
	public void clearMap(){
		Transform temp;
		while(true){
			temp=go.transform.Find("HexPrefab(Clone)");
			if(temp==null) break;
			GameObject tempGO = temp.gameObject;
			GameObject.Destroy(temp);
		}
	}
	public void update(EditorModel obj){
		clearMap();
		drawMap(obj);
	}
}