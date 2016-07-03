using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using MiniJSON;
using System.Linq;
using UnityEngine.UI;

public class Formatter : MonoBehaviour {
	[SerializeField]
	Text text;
	public string fileName;
	ParticleSystem ps;
	List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
	public Vector3 adjustPosition;
	public float adjustScale;
	public float pointSize;
	public Color pointColor;
	List<Vector3> points = new List<Vector3> ();
	public int particleCount;
	public int destinationVertexNUM;
	[SerializeField]
	string inDir;
	[SerializeField]
	string outDir;
	[SerializeField]
	string outFileName;

	void Awake()
	{
		ps = GetComponent<ParticleSystem> ();
		ps.Stop ();
		var path = Application.streamingAssetsPath + "/" + inDir + "/" + fileName;
		if (File.Exists (path)) {
			var text = File.ReadAllText (path);
			var json = (IDictionary)Json.Deserialize (text);
			var bindData = (IList)json ["data"];
			if (bindData != null) {
				foreach (IList data in bindData) {
					for(var i = 0; i < data.Count; i++){
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
			}
		}

		print (particles.Count);
	}

	void Export()
	{
		List<float[]> dataList = new List<float[]> ();
		var points = new List<Vector3> ();
		foreach(ParticleSystem.Particle particle in particles){
			points.Add (particle.position);
		}
		var vertices = points.OrderBy (e => e.y).ThenBy (e => e.z).ThenBy (e => e.x).ToList ();
		for(var i = 0; i < vertices.Count; i++){
			dataList .Add (new float[]{vertices[i].x,vertices[i].y,vertices[i].z});
		}
		Hashtable result = new Hashtable ();
		result.Add ("data",dataList);
		var resultText = Json.Serialize (result);
		File.WriteAllText (Application.streamingAssetsPath + "/" + outDir + "/" + outFileName,resultText);
		print ("complete export");
	}

	// Use this for initialization
	void Start () {
	
	}

	public void Apply()
	{
		if (destinationVertexNUM > 0) {
			var step = (float)points.Count / (float)destinationVertexNUM;

			var preParticlesCount = particles.Count;
			particles = new List<ParticleSystem.Particle> ();
			for (var i = 0.0f; i < points.Count; i += step) {
				ParticleSystem.Particle particle = new ParticleSystem.Particle ();
				Vector3 pos = new Vector3 (points [(int)i].x * adjustScale, points [(int)i].y * adjustScale, points [(int)i].z * adjustScale);
				particle.position = pos;
				particle.startSize = pointSize;
				particle.startColor = pointColor;
				particles.Add (particle);
			}

			if (particles.Count > destinationVertexNUM) {
				var particlesCount = particles.Count;
				for (var i = 0; i < particlesCount - destinationVertexNUM; i++) {
					particles.RemoveAt (particles.Count - 1);
				}
			}else if(particles.Count < destinationVertexNUM){
				for (var i = 0; i < destinationVertexNUM - particles.Count; i++) {
					ParticleSystem.Particle particle = new ParticleSystem.Particle ();
					Vector3 pos = new Vector3 (points [(int)UnityEngine.Random.Range(0,points.Count)].x * adjustScale, points [(int)UnityEngine.Random.Range(0,points.Count)].y * adjustScale, points [(int)UnityEngine.Random.Range(0,points.Count)].z * adjustScale);
					particle.position = pos;
					particle.startSize = pointSize;
					particle.startColor = pointColor;
					particles.Add (particle);
				}

			}
		} else {
			particles = new List<ParticleSystem.Particle> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		

		if(particles.Count > 0){
			ps.SetParticles (particles.ToArray(),particles.Count);
		}
		text.text = "ParticeCount :" + particles.Count.ToString();

		if(Input.GetKeyDown("e")){
			Export ();
		}
	
	}
}
