using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point{
	public int COLUMN;
	public int ROW;

	public Point(int col, int row){
		COLUMN = col;
		ROW = row;
	}
	
}
public class HexMap : EditorObserver{

	// Use this for initialization
	private GameObject HexPrefab;
	private Material[] HexMaterials;
	private GameObject go;

	private Dictionary<Point, GameObject> map;
	
	public HexMap (EditorActivity context) {
		HexPrefab = context.HexPrefab;
		HexMaterials = context.HexMaterials;
		go = GameObject.Find("HexMap");
		map = new Dictionary<Point, GameObject>();
		updateModel(context.em);
		drawMap(context.em);
		StaticBatchingUtility.Combine(go);
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
		int i = 0;
		foreach(HexModel hmodel in em.hexes){
			GameObject hexGO = GameObject.Instantiate(HexPrefab, hmodel.Position(), Quaternion.identity, go.transform);
			map.Add(new Point(hmodel.Q, hmodel.R), hexGO );
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			mr.material = HexMaterials[0];
			hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0}, {1}", hmodel.Q, hmodel.R);
			i++;
		}
	}
	public void clearMap(EditorModel em){
		int len = em.hexes.Count;
		/* for (int i =0;i<len;i++){
			if(em.hexes[i].R<em.height&&em.hexes[i].Q<em.width){
				GameObject.Destroy(map[i]);
			}
		}*/
		/* foreach(GameObject hex in map){
			GameObject.Destroy(hex);
		}*/
		map.Clear();
	}
	public void update(EditorModel obj){
		updateModel(obj);
	}
}