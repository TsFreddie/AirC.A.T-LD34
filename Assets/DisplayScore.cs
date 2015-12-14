using UnityEngine;
using System.Collections;

public class DisplayScore : MonoBehaviour {
    public UnityEngine.UI.Text Score;
	void Start () {
        Score.text = ResourceManager.Instance.finalScore.ToString();
    }
	
}
