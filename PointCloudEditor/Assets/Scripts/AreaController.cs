using UnityEngine;
using System.Collections;

public class AreaController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetScale(Vector3 scale)
	{
		this.transform.localScale = scale;
	}
}
