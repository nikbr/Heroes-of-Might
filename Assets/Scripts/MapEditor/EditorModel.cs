using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapData {
    public int emHeight;
	public int emWidth;
	public List<HexModel> emHexes = new List<HexModel>();
    public MapData(int height, int width, List<HexModel> hexes){
        emHeight = height;
		emWidth = width;
		emHexes = hexes;
    }
}

public class EditorModel  {
	private List<EditorObserver> observers = new List<EditorObserver>();
	public List<HexModel> hexes = new List<HexModel>();
	public Tool currentTool = new Tool();

	public int width;
	public int height;
	public  EditorModel (EditorActivity ea, int width, int height) {
		this.width = width;
		this.height = height;
	}

	public void addObserver(EditorObserver eo){
		observers.Add(eo);
	}

	public int totalHexes(){
		return width*height;
	}

	public void notifyObservers(){
		foreach (EditorObserver obs in observers){
			obs.update(this);
		}
	}
}


