using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum EnemyType
    {
        RapidPassby,
        BulletFirer,
        BigGuy,
    }
    public float _boundWidth = 20f;
    public float _boundHeight = 12f;

    public int Score
    {
        get
        {
            return score;
        }
    }

    public GameObject HitEffect;

    private int health = 1;
    private float speed = 1.2f;
    private bool facingRight = false;

    private bool initialized = false;
    private Vector3 movingDirection;
    private EnemyType type;

    private float invisibleTime = 0f;
    private float nextBulletTime = 1f;
    private int score = 0;

    private SpriteRenderer sprite;
    private AudioSource audio;
    private ProjectileArranger projectiles;

    public void Init(EnemyType type, int health, float speed, bool downward, bool facingRight, int score)
    {
        this.score = score;
        this.facingRight = facingRight;
        this.health = health;
        this.speed = speed;
        this.type = type;
        nextBulletTime = Random.Range(1f, 3f);
        Animator anim = GetComponent<Animator>();
        if (anim != null && anim.isInitialized)
        {
            
            switch (type)
            {
                case EnemyType.RapidPassby:
                    anim.SetTrigger("rapid");
                    break;
                case EnemyType.BulletFirer:
                    anim.SetTrigger("firer");
                    break;
                case EnemyType.BigGuy:
                    anim.SetTrigger("bigguy");
                    break;
            }
           
        }
        if (downward)
        {
            movingDirection = new Vector3(0, -1, 0);
        }
        else
        {
            movingDirection = new Vector3(0, 1, 0);
        }
        if (facingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        Color enemyColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 0.8f, 1);
        sprite.color = enemyColor;
        initialized = true;
    }

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        projectiles = FindObjectOfType<ProjectileArranger>();
    }

    void Start()
    {
        if (!initialized)
        {
            Destroy(gameObject);
        }
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            switch (type)
            {
                case EnemyType.RapidPassby:
                    anim.SetTrigger("rapid");
                    break;
                case EnemyType.BulletFirer:
                    anim.SetTrigger("firer");
                    break;
                case EnemyType.BigGuy:
                    anim.SetTrigger("bigguy");
                    break;
            }
        }

    }

    void Update()
    {
        transform.position = transform.position + movingDirection * Time.deltaTime * speed;
        float halfBoundWidth = _boundWidth / 2f;
        float halfBoundHeight = _boundHeight / 2f;

        if (transform.position.x > halfBoundWidth || transform.position.x < -halfBoundWidth || transform.position.y > halfBoundHeight || transform.position.y < -halfBoundHeight)
            enabled = false;

        #region Invisiblity

        if (invisibleTime >= 0)
        {
            invisibleTime -= Time.deltaTime;
        }
        if (invisibleTime > 0f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }

        #endregion

        #region firerLogic
        if (type == EnemyType.BulletFirer)
        {
            nextBulletTime -= Time.deltaTime;
            if (nextBulletTime <= 0)
            {
                Projectile newProjectile = projectiles.CreateProjectile(Projectile.ProjectileType.Stone, 1, 1.5f, facingRight, true);
                newProjectile.transform.position = transform.position + new Vector3(0, -0.4f, 0);
                nextBulletTime = Random.Range(1f, 3f);
            }
        }
        #endregion
    }
    public bool Hit(int damage, bool nuke = false)
    {
        if (invisibleTime > 0) return false;

        health -= damage;
        Instantiate(HitEffect, transform.position, Quaternion.AngleAxis(Random.Range(0,360), transform.forward));
        if (!nuke) audio.Play();
        if (health <= 0)
        {
            transform.position = new Vector3(100, 100, 100);
            enabled = false;
            return true;
        }
        invisibleTime = 0.2f;
        return false;
    }

}
