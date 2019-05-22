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
		defaultMap(context);
		StaticBatchingUtility.Combine(go);
	}

	// Update is called once per frame
	public void defaultMap(EditorActivity context){
		foreach(HexModel hmodel in context.em.getHexes()){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			mr.material = HexMaterials[0];
		}
	}
	public void update(EditorModel obj){

	}
}