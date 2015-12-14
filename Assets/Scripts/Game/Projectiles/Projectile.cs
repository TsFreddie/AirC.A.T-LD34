using UnityEngine;

public class Projectile : MonoBehaviour {
    public enum ProjectileType
    {
        Stone,
        Arrow,
        Suicider,
    }

    public float _boundWidth = 20f;
    public float _boundHeight = 12f;

    public Sprite _stoneSprite;
    public Sprite _arrowSprite;
    public Sprite _suiciderSprite;
    public Sprite _dangerousSprite;

    public float HitBoxSize
    {
        get
        {
            if (type == ProjectileType.Suicider)
            {
                return 1f;
            }
            return 0.3f;
        }
    }

    public int Damage
    {
        get
        {
            if (type == ProjectileType.Stone)
                Disable();
            return damage;
        }
    }

    public bool IsDangerous
    {
        get
        {
            return isDangerous;
        }
    }

    private bool isDangerous = false;
    private int damage = 1;
    private float speed = 20f;
    private bool initiailzed = false;
    private Vector3 movingDirection;
    private ProjectileType type;

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (!initiailzed)
            Destroy(gameObject);
    }

    void Update () {
        transform.position = transform.position + movingDirection * Time.deltaTime * speed;
        float halfBoundWidth = _boundWidth / 2f;
        float halfBoundHeight = _boundHeight / 2f;

        if (transform.position.x > halfBoundWidth || transform.position.x < -halfBoundWidth || transform.position.y > halfBoundHeight || transform.position.y < -halfBoundHeight)
            Disable();
    }

    public void Init(ProjectileType type, int damage, float speed, bool right, bool isDangerous)
    {
        this.type = type;
        if (right)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        if (isDangerous)
        {
            if (type == ProjectileType.Suicider)
            {
                sprite.sprite = _suiciderSprite;
            }
            else
            {
                sprite.sprite = _dangerousSprite;
            }
        }
        else if (type == ProjectileType.Stone)
        {
            sprite.sprite = _stoneSprite;
        }
        else
        {
            sprite.sprite = _arrowSprite;
        }
        this.isDangerous = isDangerous;
        this.damage = damage;
        this.speed = speed;
        if (right)
            movingDirection = new Vector3(1, 0, 0);
        else
            movingDirection = new Vector3(-1, 0, 0);
        initiailzed = true;
    }

    public void Disable()
    {
        transform.position = new Vector3(-100, -100, -100);
        gameObject.SetActive(false);
    }

}
