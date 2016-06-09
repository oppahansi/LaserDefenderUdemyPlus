using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

    public float radius = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
