using UnityEngine;
using System.Collections;

public class Nuke : MonoBehaviour {

    SpriteRenderer sprite;
    public bool died = false;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        died = false;
    }

    public void Boom()
    {
        sprite.color = new Color32(255, 220, 144, 255);
    }

    public void Hurt()
    {
        if (died) return;
        sprite.color = new Color32(255, 0, 0, 100);
    }

    public void Die()
    {
        sprite.color = new Color32(255, 255, 255, 255);
        died = true;
    }

    void Update()
    {
        Color tempColor = sprite.color;
        if (tempColor.a > 0)
        {
            if (died) tempColor.a -= Time.deltaTime * 0.2f;
            else
                tempColor.a -= Time.deltaTime * 2f;
        }
        if (tempColor.a <= 0)
        {
            if (died) ResourceManager.Instance.LoadScene(2);
            tempColor.a = 0;
        }
        sprite.color = tempColor;


    }
}
