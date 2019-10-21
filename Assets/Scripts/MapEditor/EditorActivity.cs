using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



[System.Serializable]
public class MapData {

     public int emHeight;
	  public int emWidth;
	  public List<HexModel> emHexes = new List<HexModel>();
	  public List<string> mList = new List<string>();

     public MapData(int height, int width, List<HexModel> hexes, List<string> mats)
     {
         emHeight = height;
			emWidth = width;
			emHexes = hexes;
			mList = mats;
     }
 
}

public class EditorActivity : MonoBehaviour {
	public EditorModel em;
	public HexMap hm;
	public Toolbar tb;
	public GameObject HexPrefab;
	public Material[] HexMaterials;
	void Start () {
		em = new EditorModel(this, 5, 5);
		tb = new Toolbar(this);
		em.addObserver(tb);
		hm = new HexMap(this);
		em.addObserver(hm);
	}

/* 
#Windows Store Apps: Application.persistentDataPath points to %userprofile%\AppData\Local\Packages\<productname>\LocalState.\
#iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents.
#Android: Application.persistentDataPath points to /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present), the path is resolved using android.content.Context.getExternalFilesDir.
*/

	public void saveMap(EditorModel em){	
		List<string> matList = new List<string>();
		//get terrain type
		Dictionary<Vector2Int, GameObject> smap = hm.getMap();
		foreach(KeyValuePair<Vector2Int,GameObject> hex in smap){
			MeshRenderer mr = hex.Value.GetComponentInChildren<MeshRenderer>();
			string type = mr.material.name.Replace("(Instance)","");;
			Debug.Log("found type: "+type); 
			matList.Add(type);
		}
	
		string destination = Application.persistentDataPath + "/map.dat";
		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		//MapData data = new MapData(heightValue, widthValue);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, new MapData(em.height, em.width, em.hexes, matList));
		file.Close();
		Debug.Log ("You have written to" + Application.persistentDataPath + "/map.dat");
	}

	public void loadMap(EditorModel em){	
 // 1
  if (File.Exists(Application.persistentDataPath + "/map.dat"))
  {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/map.dat", FileMode.Open);
		MapData save = (MapData)bf.Deserialize(file);
		file.Close();

		int i = 0;
		foreach(HexModel hmodel in save.emHexes){
			Debug.Log("loaded type for cell "+i+save.mList[i]);
			hmodel.type = save.mList[i];
			i++;
		} 

			tb.setHeight(save.emHeight);
			tb.setWidth(save.emWidth);

			hm.clearMap(em);
			em.hexes = save.emHexes;
			hm.drawMap(em);
			Debug.Log("Map Loaded");
	}
	else{
			Debug.Log("No map saved!");
		}
	}

	public LayerMask LayerIDForHexTiles;
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int layerMask =LayerIDForHexTiles.value;
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
				GameObject go = hit.rigidbody.gameObject;
				MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
				mr.material = HexMaterials[em.currentTool.value];
			}else{

			}
		}
	}
}
