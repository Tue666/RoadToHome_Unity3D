using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalFX
{
	public class FX_ScaleUp : MonoBehaviour
	{

		private Vector3 scaleTemp;
		public float ScaleSpeed= 30;
		private bool Active = true;

		void Start ()
		{
			scaleTemp = this.transform.localScale;
			this.transform.localScale = Vector3.zero;
		}

		void Update ()
		{
			if (Active) {
				this.transform.localScale = Vector3.Lerp (this.transform.localScale, scaleTemp, Time.deltaTime * ScaleSpeed);
			}
			if (Vector3.Magnitude (this.transform.localScale - scaleTemp) < 0.01f) {
				Active = false;
			}
		}
	}
}
