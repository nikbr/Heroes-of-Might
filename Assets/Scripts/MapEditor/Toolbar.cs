using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : EditorObserver {
	private GameObject go;
	private InputField widthInputField;
	private int widthValue;
	private InputField heightInputField;
	private int heightValue;

	private Dropdown terrainDropdown;
	private int terrainValue;
	public Toolbar(EditorActivity context){
		go = GameObject.Find("Toolbar");
		populateToolbar(context);
		terrainValue = terrainDropdown.value;
		terrainDropdown.onValueChanged.AddListener(delegate{
			terrainValue = terrainDropdown.value;
			context.em.notifyObservers();
		});
		widthInputField.onEndEdit.AddListener(delegate{
			if(widthInputField.text!=""){ //TODO add more restrictions so its only numbers are acceppted
				widthValue = int.Parse(widthInputField.text);
			}
			context.em.notifyObservers();
			context.hm.updateMap(context.em);
		});
		heightInputField.onEndEdit.AddListener(delegate{
			if(heightInputField.text!=""){ //TODO add more restrictions so its only numbers are acceppted
				heightValue = int.Parse(heightInputField.text);
			}
			context.em.notifyObservers();
			context.hm.updateMap(context.em);
		});
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
