using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditorModel  {
	private List<EditorObserver> observers = new List<EditorObserver>();
	private List<HexModel> hexes = new List<HexModel>();
	public Tool currentTool = new Tool();

	public  EditorModel (EditorActivity ea) {
		for (int col = 0;col<10;col++){
			for(int row =0;row<10;row++){
				hexes.Add(new HexModel(col, row));
			}
		}
	}

	public void addObserver(EditorObserver eo){
		observers.Add(eo);
	}

	public List<HexModel> getHexes(){return hexes;}
	public void notifyObservers(){
		foreach (EditorObserver obs in observers){
			obs.update(this);
		}
	}
}


