using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PointCloudController : MonoBehaviour {
	public Transform cube;
	ParticleSystem ps;
	public Vector3 adjustPosition;
	public float adjustScale;
	List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
	List<Vector3> points = new List<Vector3> ();
	public float pointSize;
	public Color pointColor;
	public bool isCutOut;
	public bool isReverse;

	void Awake()
	{
		ps = GetComponent<ParticleSystem> ();
		ps.Stop ();
		pointColor = new Color (UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f),UnityEngine.Random.Range(0.0f,1.0f));
	}

	// Use this for initialization
	void Start () {
	
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
		print (particles.Count);
	}

	public IList GetPointList()
	{
		List<float[]> result = new List<float[]> ();
		var points = new List<Vector3> ();
		foreach(ParticleSystem.Particle particle in particles){
			points.Add (particle.position);
		}
		//var vertices = points.OrderBy (e => e.y).ThenBy (e => e.z).ThenBy (e => e.x).ToList ();
		var vertices = points;
		for(var i = 0; i < vertices.Count; i++){
			result.Add (new float[]{vertices[i].x,vertices[i].y,vertices[i].z});
		}
		return result;
	}
	
	// Update is called once per frame
	void Update () {
		particles = new List<ParticleSystem.Particle>();
		for(var i = 0; i < points.Count; i++){
			var reverse = isReverse ? -1.0f : 1.0f; 
			Vector3 pos = new Vector3 (reverse*points [i].x*adjustScale,points [i].y*adjustScale,points [i].z*adjustScale);
			//cubeの内側にあるか
			if(isCutOut){
				if(pos.x > cube.position.x - cube.localScale.x/2.0f && pos.x < cube.position.x + cube.localScale.x/2.0f){
					if(pos.y > cube.position.y - cube.localScale.y/2.0f && pos.y < cube.position.y + cube.localScale.y/2.0f){
						if(pos.z > cube.position.z - cube.localScale.z/2.0f && pos.z < cube.position.z + cube.localScale.z/2.0f){
							ParticleSystem.Particle particle = new ParticleSystem.Particle ();
							particle.position = pos;
							particle.startSize = pointSize;
							particle.startColor = pointColor;
							particles.Add (particle);
						}
					}
				}
			}else{
				ParticleSystem.Particle particle = new ParticleSystem.Particle ();
				particle.position = pos;
				particle.startSize = pointSize;
				particle.startColor = pointColor;
				particles.Add (particle);
			}

		}

		if(particles.Count > 0){
			ps.SetParticles (particles.ToArray(),particles.Count);
		}

	}
}
