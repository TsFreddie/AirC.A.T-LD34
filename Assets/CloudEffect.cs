using UnityEngine;
using System.Collections.Generic;

public class CloudEffect : MonoBehaviour {

    public Sprite[] Clouds;
    public GameObject _cloudObj;

    public float alpha = 1f;

    private List<GameObject> cloudList;

    private float nextCloudTime = 0.5f;

    void Awake()
    {
        cloudList = new List<GameObject>();
    }

    void Update()
    {
        nextCloudTime -= Time.deltaTime;
        if (nextCloudTime < 0)
        {
            GameObject newCloud = CreateCloud();
            newCloud.transform.position = new Vector3(Random.Range(-9f, 9f), 6, 0);
            float randomScale = Random.Range(1f, 3f);
            newCloud.transform.localScale = new Vector3(randomScale, randomScale, 1);
            newCloud.GetComponent<SpriteRenderer>().sprite = Clouds[Random.Range(0, Clouds.Length)];
            nextCloudTime = Random.Range(2f, 5f);
        }
        foreach (GameObject c in cloudList)
        {
            c.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        }
    }
    public GameObject CreateCloud()
    {
        foreach (GameObject c in cloudList)
        {
            if (!c.gameObject.activeSelf)
            {
                c.gameObject.SetActive(true);
                return c;
            }
        }
        GameObject newCloud = Instantiate(_cloudObj);
        cloudList.Add(newCloud);
        return newCloud;
    }
}
