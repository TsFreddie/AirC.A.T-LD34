using UnityEngine;
using UnityEngine.UI;

public class SyncText : MonoBehaviour {

    public Text thisText;
    public Text text;

    void Update () {
        text.text = thisText.text;
    }
}
