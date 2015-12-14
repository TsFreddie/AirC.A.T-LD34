using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {

    public SpriteRenderer CatSprite;
    public SpriteRenderer CatBody;
    public SpriteRenderer CatTail;

    void Start () {
        CatSprite.sprite = ResourceManager.Instance.CarTexture[ResourceManager.Instance.selectedColor];
        CatBody.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
        CatTail.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
    }
	
	public void PrevColor()
    {
        ResourceManager.Instance.selectedColor -= 1;
        if (ResourceManager.Instance.selectedColor < 0)
            ResourceManager.Instance.selectedColor = 0;
        CatSprite.sprite = ResourceManager.Instance.CarTexture[ResourceManager.Instance.selectedColor];
        CatBody.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
        CatTail.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
    }

    public void NextColor()
    {
        ResourceManager.Instance.selectedColor += 1;
        if (ResourceManager.Instance.selectedColor >= ResourceManager.Instance.CarTexture.Length)
            ResourceManager.Instance.selectedColor = ResourceManager.Instance.CarTexture.Length - 1;
        CatSprite.sprite = ResourceManager.Instance.CarTexture[ResourceManager.Instance.selectedColor];
        CatBody.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
        CatTail.color = ResourceManager.Instance.CarColor[ResourceManager.Instance.selectedColor];
    }
}
