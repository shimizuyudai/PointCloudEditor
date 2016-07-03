using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using MiniJSON;

public class BundleManager : MonoBehaviour {
	public string inDir;
	public string outDir;
	public string outFileName;

	public string[] files;
	public GameObject prefab;
	public List<PointCloud> pointCloudList;

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
					var ctrl = obj.GetComponent<PointCloud> ();
					ctrl.Init (data);
					pointCloudList.Add (ctrl);
				}
			}
		}
	}

	void Export()
	{
		List<IList> dataList = new List<IList> ();
		foreach(var ctrl in pointCloudList){
			dataList.Add (ctrl.GetData ());
		}
		Hashtable result = new Hashtable ();
		result.Add ("data",dataList);
		var resultText = Json.Serialize (result);
		File.WriteAllText (Application.streamingAssetsPath + "/" + outDir + "/" + outFileName,resultText);
	}


	void Start () {
	
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)){
			Export ();
		}
	}
}
