using UnityEngine;
using UnityEngine.UI;

public class ProgressFilling : MonoBehaviour {
    private Image fill;
    public bool _changeColor = false;

	void Awake () {
        fill = GetComponent<Image>();
    }

    // Test
    /*void Update()
    {
        fill.fillAmount += Time.deltaTime;
        if (fill.fillAmount >= 1f)
        {
            fill.fillAmount = 0f;
        }

    }*/

    public void UpdateFillAmount(float fillAmount)
    {
        if (_changeColor) fill.color = new Color32(255, 255, 255, 255);
        if (fillAmount < 0f)
        {
            fill.fillAmount = 0;
            return;
        }
        if (fillAmount >= 1f)
        {
            fill.fillAmount = 1f;
            if (_changeColor) fill.color = new Color32(0, 255, 0, 255);
            return;
        }
        
        fill.fillAmount = fillAmount;
    }
}
