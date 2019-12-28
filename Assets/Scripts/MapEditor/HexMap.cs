using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;

	private Dictionary<Vector2Int, GameObject> map;
	
	public HexMap (EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		map = new Dictionary<Vector2Int, GameObject>();
		updateModel(context.em);
		drawMap(context.em);
		StaticBatchingUtility.Combine(go);
	}

	public Dictionary<Vector2Int, GameObject> getMap(){
		return map;
	}

	public void updateModel(EditorModel em){
		em.hexes = new List<HexModel>();
		for(int row =0;row<em.height;row++){
			for (int col = -row/2;col<em.width-row/2;col++){
				em.hexes.Add(new HexModel(col, row));
			}
		}
	}


	public void drawMap(EditorModel em){ //map is left to right row by row bottom up
		int i = 0;
		//Debug.Log("-------new cells--------");
		foreach(HexModel hmodel in em.hexes){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			map.Add(new Vector2Int(hmodel.Q, hmodel.R), hexGO );
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			//Debug.Log("drawing cell number : "+ i + "type:" + hmodel.type + "coordinates: " + string.Format("{0}, {1}", hmodel.Q, hmodel.R)); 
			i++;
			if (hmodel.type.Replace(" ", string.Empty) == "Water"){
				mr.material = HexMaterials[1];
			}else{
				mr.material = HexMaterials[0];
			}
			hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
		}
	}

	public void clearMap(EditorModel em){
		foreach(KeyValuePair<Vector2Int,GameObject> hex in map){
			GameObject.Destroy(hex.Value);
		}
		map.Clear();
	}

	public void updateMap(EditorModel em){
		List<Vector2Int> toRemove = new List<Vector2Int>();
		foreach(KeyValuePair<Vector2Int,GameObject> hex in map){
			if(hex.Key.x+hex.Key.y/2>em.width-1||hex.Key.y>em.height-1){
				GameObject.Destroy(hex.Value);
				toRemove.Add(hex.Key);
			}
		}
		//need to remove from dictionary not while iterating through it
		foreach(Vector2Int v in toRemove){
			map.Remove(v);
		}
		
		foreach(HexModel hmodel in em.hexes){
			Vector2Int p = new Vector2Int(hmodel.Q, hmodel.R);
			if(!map.ContainsKey(p)){
				GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
				map.Add(new Vector2Int(hmodel.Q, hmodel.R), hexGO );
				MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
				mr.material = HexMaterials[0];
				hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
			}
		}
	}
	public void update(EditorModel obj){
		updateModel(obj);
	}
}