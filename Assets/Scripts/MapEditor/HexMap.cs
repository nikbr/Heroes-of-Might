using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;

	private Dictionary<GameObject, HexModel> map;
	
	public HexMap (EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		map = new Dictionary<GameObject, HexModel>();
		createHexModels(context.em);
		drawMap(context.em);
		StaticBatchingUtility.Combine(go);
	}

	public Dictionary<GameObject, HexModel> getMap(){
		return map;
	}

	public void createHexModels(EditorModel em){
		em.hexes = new List<HexModel>();
		for(int row =0;row<em.height;row++){
			for (int col = -row/2;col<em.width-row/2;col++){
				em.hexes.Add(new HexModel(col, row));
			}
		}
	}

	public void updateHexModels(EditorModel em){
		int len = em.hexes.Count;
		
		if(em.height*em.width>len){
			int maxR = -10;
			int maxC = -10;
			foreach(HexModel hmodel in em.hexes){
				maxR = Math.Max(maxR, hmodel.R);
				maxC = Math.Max(maxC, hmodel.Q);
			}
			for(int row =0;row<em.height;row++){
				for (int col = -row/2;col<em.width-row/2;col++){
					if(col>maxC-row/2||row>maxR){
						em.hexes.Add(new HexModel(col, row));
					}
				}
			}
		}else if(em.height*em.width<len){
			List<HexModel> toRemove = new List<HexModel>();
			int maxR = em.height-1;
			int maxC = em.width-1;
			foreach(HexModel hmodel in em.hexes){
				if(hmodel.Q>maxC-hmodel.R/2||hmodel.R>maxR){
					Debug.Log(hmodel.Q + "," + hmodel.R);
					toRemove.Add(hmodel);
				}
			}
			foreach(HexModel hmodel in toRemove){
				em.hexes.Remove(hmodel);
			}
		}

	}



	public void drawMap(EditorModel em){
		foreach(HexModel hmodel in em.hexes){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			map.Add(hexGO, hmodel);
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			Debug.Log("drawing: "+hmodel.type); 
			if (hmodel.type.Replace(" ", string.Empty) == HexMaterials[1].name){
				mr.material = HexMaterials[1];
			}else if (hmodel.type.Replace(" ", string.Empty) == HexMaterials[0].name){
				mr.material = HexMaterials[0];
			}

			hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
		}
	}

	public void clearMap(EditorModel em){
		foreach(KeyValuePair<GameObject, HexModel> hex in map){
			GameObject.Destroy(hex.Key);
		}
		map.Clear();
	}

	public void updateMap(EditorModel em){
		Debug.Log("map updating");
		List<GameObject> toRemove = new List<GameObject>();
		foreach(KeyValuePair<GameObject, HexModel> hex in map){
			//Debug.Log("X: " + hex.Value.Q + " Y: " + hex.Value.R + " H: " + em.height + " W: " + em.width );
			if(hex.Value.Q+hex.Value.R/2>em.width-1||hex.Value.R>em.height-1){
				GameObject.Destroy(hex.Key);
				toRemove.Add(hex.Key);
			}
		}
		
		foreach(GameObject g in toRemove){
			map.Remove(g);
		}

		foreach(HexModel hmodel in em.hexes){
			if(!ContainsValue(hmodel)){
				GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
				map.Add(hexGO, hmodel);
				MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
				mr.material = HexMaterials[em.currentTool.materialToValue[hmodel.type]];
				hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
			}
		}
	}

	private bool ContainsValue(HexModel h){
		foreach(KeyValuePair<GameObject, HexModel> hex in map){
			if (h.Q==hex.Value.Q&&h.R==hex.Value.R){
				return true;
			}
		}
		return false;
	}
	public void update(EditorModel obj){
		updateHexModels(obj);
		updateMap(obj);
	}
}