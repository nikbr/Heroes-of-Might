using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditorModel  {
	private List<EditorObserver> observers = new List<EditorObserver>();

	private List<HexModel> hexes = new List<HexModel>();
	public  EditorModel () {
		for (int col = 0;col<10;col++){
			for(int row =0;row<10;row++){
				hexes.Add(new HexModel(col, row));
			}
		}
	}

	public List<HexModel> getHexes(){return hexes;}
	public void notifyObservers(){
		foreach (EditorObserver obs in observers){
			obs.update(this);
		}
	}
}


