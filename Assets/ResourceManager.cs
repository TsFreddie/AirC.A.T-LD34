using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour {

    public Sprite[] CarTexture;
    public Color[] CarColor;

    public int selectedColor = 0;
    public int finalScore = 0;

    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

	void Awake () {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(gameObject);
        }
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
	
}
