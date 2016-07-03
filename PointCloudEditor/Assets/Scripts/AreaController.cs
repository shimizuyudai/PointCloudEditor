using UnityEngine;
using System.Collections;

public class AreaController : MonoBehaviour {
	Material material;

	void Awake()
	{
		material = GetComponent<Renderer> ().material;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(Color color)
	{
		material.color = color;
	}

	public void SetScale(Vector3 scale)
	{
		this.transform.localScale = scale;
	}
}
