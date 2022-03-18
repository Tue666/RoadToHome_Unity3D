using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalFX
{
	public class FX_MoveToTarget : MonoBehaviour
	{

		public Vector3 TargetPosition;
		public bool UseRayCase;
		public float DampingStart = 30;
		public float DampingSpeed = 10;
		public float Speed = 10;
		public Vector3 SpreadMin;
		public Vector3 SpreadMax;
		private Rigidbody rigidBody;

		void Start ()
		{
			rigidBody = this.GetComponent<Rigidbody> ();

			if (UseRayCase) {
				RaycastHit hit;
				if (Physics.Raycast (this.transform.position, this.transform.forward, out hit)) {
					TargetPosition = hit.point;
				}
			}
			Vector3 dir = new Vector3 (Random.Range (SpreadMin.x, SpreadMax.x), Random.Range (SpreadMin.y, SpreadMax.y), Random.Range (SpreadMin.z, SpreadMax.z)) * 0.01f;
			Vector3 dirmod = (this.transform.forward * dir.z) + (this.transform.right * dir.x) + (this.transform.up * dir.y);
			//dirmod.x *= dir.x;
			//dirmod.y *= dir.y;
			//dirmod.z *= dir.z;
			this.transform.forward = dirmod;
		}


		void Update ()
		{
			rigidBody.velocity = new Vector3 (rigidBody.velocity.x, 0, rigidBody.velocity.z);
			DampingStart += DampingSpeed * Time.deltaTime;
			Quaternion rotationTarget = Quaternion.LookRotation ((TargetPosition - this.transform.position).normalized);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, rotationTarget, Time.deltaTime * DampingStart);
			this.transform.position += this.transform.forward * Speed * Time.deltaTime;
		}
	}

}