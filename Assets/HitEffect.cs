using UnityEngine;

public class HitEffect : MonoBehaviour {

    SpriteRenderer sprite;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color tempColor = sprite.color;
        if (tempColor.a > 0)
        {
            tempColor.a -= Time.deltaTime * 1.5f;
        }
        if (tempColor.a <= 0)
        {
            tempColor.a = 0;
            Destroy(gameObject);
        }
        sprite.color = tempColor;


    }
}
