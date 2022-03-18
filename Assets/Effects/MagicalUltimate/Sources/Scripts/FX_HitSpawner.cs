using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_HitSpawner : MonoBehaviour
	{


		public GameObject FXSpawn;
		public bool DestoyOnHit = false;
		public bool FixRotation = false;
		public float LifeTimeAfterHit = 1;
		public float LifeTime = 0;
		public ParticleSystem[] Particles;
		public Renderer[] Renderers;
	
		void Start ()
		{
			if (Particles.Length <= 0) {
				Particles = (ParticleSystem[])this.transform.GetComponentsInChildren <ParticleSystem> ();
			}
		}
	
		void Spawn ()
		{
			if (FXSpawn != null) {
				Quaternion rotate = this.transform.rotation;
				if (!FixRotation)
					rotate = FXSpawn.transform.rotation;
				GameObject fx = (GameObject)GameObject.Instantiate (FXSpawn, this.transform.position, rotate);
				if (LifeTime > 0)
					GameObject.Destroy (fx.gameObject, LifeTime);
			}
			if (DestoyOnHit) {
				
				for (int i = 0; i < Particles.Length; i++) {
					Particles [i].Stop ();
				}
				for (int i = 0; i < Renderers.Length; i++) {
					Renderers [i].enabled = false;
				}
				GameObject.Destroy (this.gameObject, LifeTimeAfterHit);
				if (this.gameObject.GetComponent<Collider>())
					this.gameObject.GetComponent<Collider>().enabled = false;

			}
		}
	
		void OnTriggerEnter (Collider other)
		{
			Spawn ();
		}
	
		void OnCollisionEnter (Collision collision)
		{
			Spawn ();
		}
	}
}