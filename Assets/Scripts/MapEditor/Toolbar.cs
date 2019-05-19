using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : EditorObserver {
	private GameObject go;
	private Dropdown terrainDropdown;
	private int terrainValue;
	public Toolbar(EditorActivity context){
		go = GameObject.Find("Toolbar");
		populateTerrainDropdown(context);
		terrainValue = terrainDropdown.value;
		terrainDropdown.onValueChanged.AddListener(delegate{
			terrainValue = terrainDropdown.value;
			context.em.notifyObservers();
		});
	}
	public void populateTerrainDropdown(EditorActivity context){
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
	}
}
