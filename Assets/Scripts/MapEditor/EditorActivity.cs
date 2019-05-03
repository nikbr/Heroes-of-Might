using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorActivity : MonoBehaviour {
	public EditorModel em;
	public HexMap hm;

	//stuff to be passed in
	public GameObject HexPrefab;
	public Material[] HexMaterials;
	void Start () {
		em = new EditorModel();
		hm = new HexMap(em, this);
	}
}
