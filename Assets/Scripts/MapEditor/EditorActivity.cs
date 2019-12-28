using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleFileBrowser;

public enum Permission { Denied = 0, Granted = 1, ShouldAsk = 2 };

public delegate void OnSuccess(string path);
public delegate void OnCancel();

[System.Serializable]
public class serialVector {
	public int ex;
	public int ey;
	public serialVector(int x, int y){
ex = x;
ey = y;
	}
}

[System.Serializable]
public class MapData {

     public int emHeight;
	  public int emWidth;
	  public List<HexModel> emHexes = new List<HexModel>();
	  public List<string> mList = new List<string>();
	  public List<serialVector> mcordsList;

     public MapData(int height, int width, List<HexModel> hexes, List<serialVector> cordsList, List<string> mats)
     {
         emHeight = height;
			emWidth = width;
			emHexes = hexes;
			mcordsList = cordsList;
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
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Maps", ".dat"));	

	}

/* 
#Windows Store Apps: Application.persistentDataPath points to %userprofile%\AppData\Local\Packages\<productname>\LocalState.\
#iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents.
#Android: Application.persistentDataPath points to /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present), the path is resolved using android.content.Context.getExternalFilesDir.
*/



	public void saveMapHelper(EditorModel em, string destination){
		List<string> matList = new List<string>();
		List<serialVector> cordsList = new List<serialVector>();
		//get terrain type
		Dictionary<Vector2Int, GameObject> smap = hm.getMap();
		Debug.Log("-------saving map--------");
		foreach(KeyValuePair<Vector2Int,GameObject> hex in smap){
			//Debug.Log(string.Format("{0}, {1}", hex.Q, hex.R));
			MeshRenderer mr = hex.Value.GetComponentInChildren<MeshRenderer>();
			string type = mr.material.name.Replace("(Instance)","");;
			Debug.Log("type:" + type + "smap coordinates: " + hex.Key); 

			//Debug.Log("found type: "+type); 
			matList.Add(type);
			cordsList.Add(new serialVector(hex.Key.x, hex.Key.y));
		}

		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		//Debug.Log( "destination is: " + destination );

		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, new MapData(em.height, em.width, em.hexes, cordsList , matList));

		file.Close();

		//Debug.Log ("You have written to" + destination);
	}

	public void saveMap(EditorModel em){
			string destination = Application.persistentDataPath + "/map.dat";
			//	bool ShowSaveDialog( OnSuccess onSuccess, OnCancel onCancel, bool folderMode = false, string initialPath = Application.persistentDataPath, string title = "Save", string saveButtonText = "Save" );
			FileBrowser.ShowSaveDialog( (path) => { saveMapHelper( em, path);  } , null, false, Application.persistentDataPath, "Save Map As", "Save Map" );
	}

	public void loadMapHelper(EditorModel em, string destination){	

  if (File.Exists(destination))
  {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(destination, FileMode.Open);
		if (file == null){
			file.Close();
			Debug.Log("Map corrupted!");
			return;
		}
		MapData save = (MapData)bf.Deserialize(file);
		file.Close();
		
		Debug.Log("-------new cells--------");
		Debug.Log(save);

		foreach( var x in save.mList) { //textures saved in correct order, order of coordinates is wrong
 			Debug.Log( x.ToString());
		}
		List<HexModel> buffer_hexes = new List<HexModel>();

		int i = 0;
		foreach(HexModel hmodel in save.emHexes){
			//Debug.Log("loaded type for cell "+i+save.mList[i]);
			int Q = save.mcordsList[i].ex;
			int R = save.mcordsList[i].ey;
			HexModel s = new HexModel(Q, R); // add new constructor to overload terrain type
			s.type = save.mList[i];
			buffer_hexes.Add(s);
			Debug.Log("type:" + save.mList[i] + "coordinates: " + string.Format("{0}, {1}", s.Q, s.R)); 

			i++;
		} 

			tb.setHeight(save.emHeight);
			tb.setWidth(save.emWidth);

			hm.clearMap(em);
			em.hexes = buffer_hexes;
			hm.drawMap(em);
			Debug.Log("Map Loaded From"+destination);
	}
	else{
			Debug.Log("No map saved!");
		}
	}

	public void loadMap(EditorModel em){
		string destination = Application.persistentDataPath + "/map.dat";
		//	bool ShowLoadDialog( OnSuccess onSuccess, OnCancel onCancel, bool folderMode = false, string initialPath = Application.persistentDataPath, string title = "Save", string saveButtonText = "Save" );
		FileBrowser.ShowLoadDialog( (path) => { loadMapHelper( em, path);  } , null, false, Application.persistentDataPath, "Load Map", "Load Map" );
}

	public LayerMask LayerIDForHexTiles;
	void Update () {
		if (Input.GetMouseButton(0)){
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
