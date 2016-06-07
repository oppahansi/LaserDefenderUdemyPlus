using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float DestroyTime = 2.0f;
    
	void Start () {
        Destroy(gameObject, DestroyTime);
	}
}
