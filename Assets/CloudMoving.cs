using UnityEngine;
using System.Collections;

public class CloudMoving : MonoBehaviour {
    private float speed = 0.3f;
    public float _boundWidth = 20f;
    public float _boundHeight = 12f;
    public float alpha = 1f;
    void Update () {
        transform.position = transform.position + new Vector3(0, -1, 0) * Time.deltaTime * speed;
        float halfBoundWidth = _boundWidth / 2f;
        float halfBoundHeight = _boundHeight / 2f;

        if (transform.position.x > halfBoundWidth || transform.position.x < -halfBoundWidth || transform.position.y > halfBoundHeight || transform.position.y < -halfBoundHeight)
            gameObject.SetActive(false);

        
    }
}
