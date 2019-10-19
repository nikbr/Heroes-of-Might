using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;

	private List<GameObject> map;
	
	public HexMap (EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		map = new List<GameObject>();
		updateModel(context.em);
		drawMap(context.em);
		StaticBatchingUtility.Combine(go);
	}

	public List<GameObject> getMap(){
		return map;
	}

	public void updateModel(EditorModel em){
		em.hexes = new List<HexModel>();
		for (int col = 0;col<em.width;col++){
			for(int row =0;row<em.height;row++){
				em.hexes.Add(new HexModel(col, row));
			}
		}
	}



	public void drawMap(EditorModel em){	
		foreach(HexModel hmodel in em.hexes){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			map.Add(hexGO);
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			Debug.Log("drawing: "+hmodel.type); 
			if (hmodel.type == "Water"){
				mr.material = HexMaterials[1];
			}else{
				mr.material = HexMaterials[0];
			}

			hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
		}
	}
	public void clearMap(){
		foreach(GameObject hex in map){
			GameObject.Destroy(hex);
		}
	}
	public void update(EditorModel obj){
		updateModel(obj);
	}
}