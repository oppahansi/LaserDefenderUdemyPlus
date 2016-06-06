using UnityEngine;
using System.Collections;

public class HowToDisplay : MonoBehaviour {

	public void DisplayHowTo()
    {
        Canvas howTo = gameObject.GetComponent<Canvas>();
        howTo.enabled = !howTo.enabled;
    }
}
