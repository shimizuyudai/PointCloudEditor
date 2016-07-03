using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloud : MonoBehaviour {
	List<Vector3> points = new List<Vector3> ();
	List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
	public float pointSize;
	public Color pointColor;
	ParticleSystem ps;

	void Awake()
	{
		ps = GetComponent<ParticleSystem> ();
		ps.Stop ();
		pointColor = new Color (UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f));
	}

	// Use this for initialization
	void Start () {
	
	}

	public IList GetData()
	{
		List<float[]> result = new List<float[]> ();
		var points = new List<Vector3> ();
		for(var i = 0; i < points.Count; i++){
			result.Add (new float[]{points[i].x,points[i].y,points[i].z});
		}
		return result;
	}

	public void Init(IList data)
	{
		for (var i = 0; i < data.Count; i++) {
			ParticleSystem.Particle particle = new ParticleSystem.Particle ();
			var pointdata = (IList)data [i];
			var x = float.Parse (pointdata [0].ToString ());
			var y = float.Parse (pointdata [1].ToString ());
			var z = float.Parse (pointdata [2].ToString ());
			particle.position = new Vector3 (x,y,z);
			points.Add (new Vector3 (x,y,z));
			particle.startColor = Color.blue;
			particle.startSize = pointSize;
			particle.startColor = pointColor;
			particles.Add (particle);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(particles.Count > 0){
			ps.SetParticles (particles.ToArray(),particles.Count);
		}
	}
}
