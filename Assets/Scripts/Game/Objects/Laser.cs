using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    SpriteRenderer sprite;
    Controller character;
    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        character = FindObjectOfType<Controller>();
    }

    public void Shoot(bool right)
    {
        if (right)
        {
            transform.position = new Vector3(4.8f, character.transform.position.y - 0.2f, 0);
        }
        else
        {
            transform.position = new Vector3(-4.8f, character.transform.position.y - 0.2f, 0);
        }
        transform.localScale = new Vector3(28, 2, 0);
        sprite.color = new Color(1, 1, 1, 1);
    }
	
	void Update ()
    {
        transform.position = new Vector3(transform.position.x, character.transform.position.y - 0.2f, transform.position.z);
        Color tempColor = sprite.color;
        Vector3 tempScale = transform.localScale;
        if (tempColor.a > 0)
        {
            tempScale.y -= Time.deltaTime * 0.5f;
            tempColor.a -= Time.deltaTime * 0.5f;
        }
            
        if (tempColor.a <= 0)
        {
            tempColor.a = 0;
        }
        transform.localScale = tempScale;
        sprite.color = tempColor;


    }
}
