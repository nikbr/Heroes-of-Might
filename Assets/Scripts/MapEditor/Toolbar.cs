using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MapData {

     public int HheightValue;
     public int HwidthValue;
 
     public MapData(int heightValue, int widthValue)
     {
         HheightValue = heightValue;
		 HwidthValue = widthValue;
     }
 
}

public class Toolbar : EditorObserver {
	private GameObject go;
	private InputField widthInputField;
	private int widthValue;
	private InputField heightInputField;
	private int heightValue;

	private Dropdown terrainDropdown;

	private Button loadButton;	 
	private Button saveButton;	 

	private int terrainValue;

	public Toolbar(EditorActivity context){
		go = GameObject.Find("Toolbar");
		populateToolbar(context);
		terrainValue = terrainDropdown.value;
		terrainDropdown.onValueChanged.AddListener(delegate{
			terrainValue = terrainDropdown.value;
			context.em.notifyObservers();
		});
		widthInputField.onValueChanged.AddListener(delegate{
			if(widthInputField.text!=""){ //TODO add more restrictions so its only numbers are acceppted
				widthValue = int.Parse(widthInputField.text);
			}
			context.hm.clearMap();
			context.em.notifyObservers();
			context.hm.drawMap(context.em);
		});
		heightInputField.onValueChanged.AddListener(delegate{
			if(heightInputField.text!=""){ //TODO add more restrictions so its only numbers are acceppted
				heightValue = int.Parse(heightInputField.text);
			}
			context.hm.clearMap();
			context.em.notifyObservers();
			context.hm.drawMap(context.em);
		});
		
		GameObject loadButtonGO = go.transform.Find("LoadButton").gameObject;
		loadButton = loadButtonGO.GetComponent<Button>();
		loadButton.onClick.AddListener(loadFunc);
		
		GameObject saveButtonGO = go.transform.Find("SaveButton").gameObject;
		saveButton = saveButtonGO.GetComponent<Button>();
		saveButton.onClick.AddListener(saveFunc);

	}
/* 
#Windows Store Apps: Application.persistentDataPath points to %userprofile%\AppData\Local\Packages\<productname>\LocalState.\
#iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents.
#Android: Application.persistentDataPath points to /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present), the path is resolved using android.content.Context.getExternalFilesDir.
*/

void saveFunc(){
	string destination = Application.persistentDataPath + "/map.dat";
	FileStream file;

	if(File.Exists(destination)) file = File.OpenWrite(destination);
	else file = File.Create(destination);

	MapData data = new MapData(heightValue, widthValue);
	BinaryFormatter bf = new BinaryFormatter();
	bf.Serialize(file, data);


	file.Close();
	Debug.Log ("You have written to map.dat to" + Application.persistentDataPath);
	}

void loadFunc(){
		Debug.Log ("You have clicked the button!");
	}
	private void populateToolbar(EditorActivity context){
		populateWidthAndHeight(context);
		populateTerrainDropdown(context);
	}
	private void populateWidthAndHeight(EditorActivity context){
		GameObject widthInputFieldGO = go.transform.Find("WidthInputField").gameObject;
		GameObject heightInputFieldGO = go.transform.Find("HeightInputField").gameObject;


		widthInputField = widthInputFieldGO.GetComponent<InputField>();
		heightInputField = heightInputFieldGO.GetComponent<InputField>();
		widthValue = context.em.width;
		widthInputField.text = widthValue.ToString();
		heightValue = context.em.height;
		heightInputField.text = heightValue.ToString();


		
	}
	private void populateTerrainDropdown(EditorActivity context){
		GameObject terrainDropdownGO =  go.transform.Find("TerrainDropdown").gameObject;
		terrainDropdown = terrainDropdownGO.GetComponent<Dropdown>();
		List<string> dropdownOptions = new List<string>();
		foreach (Material mat in context.HexMaterials){
			dropdownOptions.Add(mat.name);	
		}
		terrainDropdown.AddOptions(dropdownOptions);
	}
	public void update (EditorModel obj) {
		obj.currentTool.value = terrainValue;
		obj.width = widthValue;
		obj.height = heightValue;
	}
}
