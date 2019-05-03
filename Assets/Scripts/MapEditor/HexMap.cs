using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;
	
	public HexMap (EditorModel em, EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		defaultMap(em, context);
		StaticBatchingUtility.Combine(go);
	}

	// Update is called once per frame
	public void defaultMap(EditorModel em, EditorActivity context){
		foreach(HexModel hmodel in em.getHexes()){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			mr.material = HexMaterials[0];
		}
	}
	public void update(EditorModel obj){

	}
}