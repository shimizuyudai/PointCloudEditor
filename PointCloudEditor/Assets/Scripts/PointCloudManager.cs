using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using MiniJSON;

public class PointCloudManager : MonoBehaviour {
	public GameObject prefab;
	List<PointCloudController> pointCloudControllerList = new List<PointCloudController>();
	[SerializeField]
	string[] files;
	public float adjustScale;
	public float pointSize;
	public Color pointColor;
	[SerializeField]
	string outFileName;
	[SerializeField]
	string inDir;
	[SerializeField]
	string outDir;

	void Awake()
	{
		foreach(var fileName in files){
			var path = Application.streamingAssetsPath + "/" + inDir + "/" + fileName;
			if (File.Exists (path)) {
				var text = File.ReadAllText (path);
				var json = (IDictionary)Json.Deserialize (text);
				var data = (IList)json ["data"];
				if (data != null) {
					var obj = GameObject.Instantiate (prefab)as GameObject;
					var ctrl = obj.GetComponent<PointCloudController> ();
					ctrl.Init (data);
					pointCloudControllerList.Add (ctrl);
				}
			}
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(var ctrl in pointCloudControllerList){
			ctrl.adjustScale = this.adjustScale;
			ctrl.pointSize = this.pointSize;
//			ctrl.pointColor = this.pointColor;
		}

		if(Input.GetKeyDown("e")){
			Export ();
		}
	}

	void Export()
	{
		List<IList> dataList = new List<IList> ();
		foreach(var ctrl in pointCloudControllerList){
			dataList.Add (ctrl.GetPointList ());
		}
		Hashtable result = new Hashtable ();
		result.Add ("data",dataList);
		var resultText = Json.Serialize (result);
		File.WriteAllText (Application.streamingAssetsPath + "/" + outDir + "/" + outFileName,resultText);
	}
}
