using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_ShakeCamera : MonoBehaviour
	{

		public Vector3 Power = Vector3.up;
		public float ShakeRate = 0;
		void Start ()
		{
			timeTmp = Time.time;
			CameraEffect.Shake (Power);
		}
		float timeTmp;
		void Update(){
			if (ShakeRate > 0) {
				if (Time.time > timeTmp + ShakeRate) {
					timeTmp = Time.time;
					CameraEffect.Shake (Power);
				}
			}
		}
	}
}
