using UnityEngine;

public class GameManager : MonoBehaviour
{

    // WeaponIntervalIndicator - eww, public variables
    public ProgressFilling _stoneInv;
    public ProgressFilling _arrowInv;
    public ProgressFilling _laserInv;
    public ProgressFilling _nukeInv;

    // WeaponActionProgressIndicator - eww, public variables
    public ProgressFilling _stonePg;
    public ProgressFilling _arrowPg;
    public ProgressFilling _laserPg;
    public ProgressFilling _nukePg;

    // SoundClip
    public AudioClip _stoneSound;
    public AudioClip _arrowSound;
    public AudioClip _laserSound;
    public AudioClip _nukeSound;

    // Animator
    public Animator _bowAnim;
    public Animator _ufoEffectAnim;

    // Objects
    public Laser _laser;
    public Nuke _nuke;
    public GameObject _warningSign;
    public GameObject _startAnimation;

    // ColorChange
    public SpriteRenderer _catSprite;

    public UnityEngine.UI.Text _scoreText;

    // Time
    public float _stoneInterval = 0.1f;
    public float _arrowInterval = 3f;
    public float _arrowChargeTime = 1f;
    public float _laserInterval = 20f;
    public float _laserChargeTime = 5f;
    public float _nukeInterval = 20f;
    public float _nukeChargeTime = 5f;

    // Bound
    public float _spawnDistance = 5.5f;

    private int totalScore = 0;

    // Components
    private Controller charControl;
    private ProjectileArranger projectiles;
    private EnemyArranger enemies;
    private AudioSource audio;
    
    // Timer
    private double time = -4.3f;
    private double stoneLastTime = 0f;
    private double arrowActionTime = 0f;
    private double arrowLastTime = 0f;
    private double laserActionTime = 0f;
    private double laserLastTime = 0f;
    private double nukeActionTime = 0f;
    private double nukeLastTime = 0f;
    private double enemyLastTime = 0f;
    private float nextEnemyInterval = 1f;
    private double firerLastTime = 0f;
    private float nextFirerInterval = 1.5f;
    private float laserTime = 0f;
    private double suiciderLastTime = 0f;
    private float nextsuiciderInterval = 10f;
    private int bigguyCount = 1;
    private float bigguyInterval = 60f;

    // Projectile direction
    private bool arrowRight = false;
    private bool laserRight = false;
    private bool laserDealingRight = false;

    void Awake () {
        charControl = FindObjectOfType<Controller>();
        projectiles = GetComponent<ProjectileArranger>();
        enemies = GetComponent<EnemyArranger>();
        audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        charControl.gameObject.SetActive(false);
        _catSprite.sprite = ResourceManager.Instance.CarTexture[ResourceManager.Instance.selectedColor];
        totalScore = 0;
    }

    bool Left()
    {
        return Input.GetKey(KeyCode.A) || Input.GetMouseButton(0 ) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.F);
    }

    bool Right()
    {
        return Input.GetKey(KeyCode.D) || Input.GetMouseButton(1 ) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.J);
    }

    bool LeftDown()
    {
        return Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(0 ) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.F);
    }

    bool RightDown()
    {
        return Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(1 ) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.J);
    }

    bool LeftUp()
    {
        return Input.GetKeyUp(KeyCode.A) || Input.GetMouseButtonUp(0 ) || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.F);
    }

    bool RightUp()
    {
        return Input.GetKeyUp(KeyCode.D) || Input.GetMouseButtonUp(1 ) || Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.J);
    }

    void Update () {
        if (_nuke.died) return;
        #region Gathering everything we need

        time += Time.deltaTime;
        if (time < 0) return;
        charControl.gameObject.SetActive(true);
        _startAnimation.gameObject.SetActive(false);
        Vector3 charPos = charControl.gameObject.transform.position;
        _scoreText.text = totalScore.ToString();

        #endregion

        #region StoneLogic
        _stoneInv.UpdateFillAmount((float)((time - stoneLastTime) / _stoneInterval));
        if (time - stoneLastTime > _stoneInterval)
        {
            _stonePg.UpdateFillAmount(0f);
            // Left        
            if (LeftDown())
            {
                charControl.transform.localScale = new Vector3(-1, 1, 1);
                Projectile newProjectile = projectiles.CreateProjectile(Projectile.ProjectileType.Stone, 1, 20f, false, false);
                newProjectile.transform.position = charPos;
                stoneLastTime = time;
                _stonePg.UpdateFillAmount(1f);
                audio.PlayOneShot(_stoneSound);
            }
            // right
            else if (RightDown())
            {
                charControl.transform.localScale = new Vector3(1, 1, 1);
                Projectile newProjectile = projectiles.CreateProjectile(Projectile.ProjectileType.Stone, 1, 20f, true, false);
                newProjectile.transform.position = charPos;
                stoneLastTime = time;
                _stonePg.UpdateFillAmount(1f);
                audio.PlayOneShot(_stoneSound);
            }
        }
        #endregion

        #region ArrowLogic
        _arrowInv.UpdateFillAmount((float)((time - arrowLastTime) / _arrowInterval));
        _arrowPg.UpdateFillAmount((float)((arrowActionTime) / _arrowChargeTime));
        if (time - arrowLastTime > _arrowInterval)
        {
            // Action
            if (Left() && Right() || (LeftUp() && RightDown()) || (RightUp() && LeftDown()))
            {
                if (arrowActionTime > _arrowChargeTime)
                {
                    Projectile newProjectile = projectiles.CreateProjectile(Projectile.ProjectileType.Arrow, 5, 30f, arrowRight, false);
                    newProjectile.transform.position = charPos + new Vector3(0, 0.1f, 0);
                    arrowLastTime = time;
                    audio.PlayOneShot(_arrowSound);
                }
                arrowActionTime = 0f;
                _bowAnim.SetBool("charged", false);
                _bowAnim.SetBool("charge", false);
            }
            // Left -> Rigth
            else if (Left())
            {
                if (arrowActionTime >= 0.15f)
                {
                    _bowAnim.SetBool("charge", true);
                    charControl.transform.localScale = new Vector3(1, 1, 1);
                }
                if (arrowActionTime <= _arrowChargeTime) arrowRight = true;
                else
                {
                    _bowAnim.SetBool("charged", true);
                }
                arrowActionTime += Time.deltaTime;
            }
            // right -> left
            else if (Right())
            {
                if (arrowActionTime >= 0.15f)
                {
                    _bowAnim.SetBool("charge", true);
                    charControl.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (arrowActionTime <= _arrowChargeTime) arrowRight = false;
                else
                {
                    _bowAnim.SetBool("charged", true);
                }
                arrowActionTime += Time.deltaTime;
            }
            if (!Left() && !Right())
            {
                _bowAnim.SetBool("charged", false);
                _bowAnim.SetBool("charge", false);
                arrowActionTime = 0f;
            }
        }
        else
        {
            _bowAnim.SetBool("charged", false);
            _bowAnim.SetBool("charge", false);
            arrowActionTime = 0f;
        }
        #endregion

        #region LaserLogic
        _laserInv.UpdateFillAmount((float)((time - laserLastTime) / _laserInterval));
        _laserPg.UpdateFillAmount((float)((laserActionTime) / _laserChargeTime));
        if (time - laserLastTime > _laserInterval)
        {
            // Action
            if (Left() && Right())
            {
                laserActionTime = 0f;
                _ufoEffectAnim.SetBool("laserCharged", false);
                _ufoEffectAnim.SetBool("laserCharge", false);
            }
            // Left
            else if (Left())
            {
                if (laserActionTime >= 0.15f) _ufoEffectAnim.SetBool("laserCharge", true);
                if (laserActionTime <= _laserChargeTime) laserRight = false;
                else
                {
                    _ufoEffectAnim.SetBool("laserCharged", true);
                }
                laserActionTime += Time.deltaTime;
            }
            // right
            else if (Right())
            {
                if (laserActionTime >= 0.15f) _ufoEffectAnim.SetBool("laserCharge", true);
                if (laserActionTime <= _laserChargeTime) laserRight = true;
                else
                {
                    _ufoEffectAnim.SetBool("laserCharged", true);
                }
                laserActionTime += Time.deltaTime;
            }
            if ((!Left() && !Right()) || (LeftUp() && RightDown()) || (RightUp() && LeftDown()))
            {
                if (laserActionTime > _laserChargeTime)
                {
                    audio.PlayOneShot(_laserSound);
                    _laser.Shoot(laserRight);
                    laserDealingRight = laserRight;
                    laserTime = 2f;
                    laserLastTime = time;
                    if (laserRight)
                        charControl.transform.localScale = new Vector3(1, 1, 1);
                    else
                        charControl.transform.localScale = new Vector3(-1, 1, 1);
                }
                laserActionTime = 0f;
                _ufoEffectAnim.SetBool("laserCharged", false);
                _ufoEffectAnim.SetBool("laserCharge", false);
            }
        }
        else
        {
            laserActionTime = 0f;
        }
        if (laserTime > 0f) laserTime -= Time.deltaTime;
        #endregion

        #region NukeLogic
        _nukeInv.UpdateFillAmount((float)((time - nukeLastTime) / _nukeInterval));
        _nukePg.UpdateFillAmount((float)((nukeActionTime) / _nukeChargeTime));
        if (time - nukeLastTime > _nukeInterval)
        {
            // Action
            if (Left() && Right())
            {
                nukeActionTime += Time.deltaTime;
                _ufoEffectAnim.SetBool("nukeCharge", true);
                if (nukeActionTime > _nukeChargeTime)
                {
                    _ufoEffectAnim.SetBool("nukeCharged", true);
                }
            }
            else
            {
                if (nukeActionTime > _nukeChargeTime)
                {
                    audio.PlayOneShot(_nukeSound);
                    nukeLastTime = time;
                    _nuke.Boom();
                    foreach (Enemy e in enemies.List)
                    {
                        if (e.gameObject.GetComponent<Enemy>().enabled)
                        {
                            if (e.Hit(10, true))
                            {
                                totalScore += e.Score;
                            }
                        }
                    }
                    foreach (Projectile p in projectiles.List)
                    {
                        if (p.gameObject.activeSelf)
                        {
                            p.Disable();
                        }
                    }
                }
                nukeActionTime = 0f;
                _ufoEffectAnim.SetBool("nukeCharge", false);
                _ufoEffectAnim.SetBool("nukeCharged", false);
            }
        }
        else
        {
            nukeActionTime = 0f;
            _ufoEffectAnim.SetBool("nukeCharge", false);
            _ufoEffectAnim.SetBool("nukeCharged", false);
        }
        #endregion

        #region EnemyGenerating

        // BulletFirer
        if (time - firerLastTime >= nextFirerInterval)
        {
            bool isFacingRight = Random.Range(0, 2) == 0 ? true : false;
            bool isDownward = Random.Range(0, 2) == 0 ? true : false;
            Enemy newEnemy = enemies.CreateEnemy(Enemy.EnemyType.BulletFirer, 8, 0.8f, isDownward, isFacingRight, 20);
            newEnemy.transform.position = new Vector3(isFacingRight ? charPos.x - 3f - Random.Range(0f, _spawnDistance - 2f) : charPos.x + 3f + Random.Range(0f, _spawnDistance - 2f), isDownward ? 5.5f : -5.5f, 0);
            firerLastTime = time;
            nextFirerInterval = Random.Range(5f, 8f);
        }

        // Rapid
        if (time - enemyLastTime >= nextEnemyInterval)
        {
            bool isFacingRight = Random.Range(0, 2) == 0 ? true : false;
            bool isDownward = Random.Range(0, 2) == 0 ? true : false;
            Enemy newEnemy = enemies.CreateEnemy(Enemy.EnemyType.RapidPassby, 1, Random.Range(2f, 6f), isDownward, isFacingRight, 1);
            newEnemy.transform.position = new Vector3(isFacingRight ? charPos.x - 1.3f - Random.Range(0f, _spawnDistance) : charPos.x + 1.3f + Random.Range(0f, _spawnDistance), isDownward ? 5.5f : -5.5f, 0);
            enemyLastTime = time;
            nextEnemyInterval = Random.Range(0.5f, 1.2f);
        }

        // Suicider [projectile]
        if (time - suiciderLastTime >= nextsuiciderInterval - 1f)
        {
            if (!_warningSign.activeSelf)
            {
                bool isFacingRight = Random.Range(0, 2) == 0 ? true : false;
                _warningSign.transform.position = new Vector3(isFacingRight ? -6 : 6, charControl.PredictY(), 0);
                _warningSign.transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
                _warningSign.SetActive(true);
            }
        }

        // Suicider
        if (time - suiciderLastTime >= nextsuiciderInterval)
        {
            Projectile newProjectile = projectiles.CreateProjectile(Projectile.ProjectileType.Suicider, 3, 15f, _warningSign.transform.position.x < 0, true);
            newProjectile.transform.position = new Vector3((_warningSign.transform.position.x < 0) ? charPos.x - 9.5f : charPos.x + 9.5f, _warningSign.transform.position.y, 0);
            suiciderLastTime = time;
            nextsuiciderInterval = Random.Range(4f, 9f);
            _warningSign.SetActive(false);
        }

        // Bigguy
        if (time >= bigguyCount * bigguyInterval)
        {
            bool isFacingRight = Random.Range(0, 2) == 0 ? true : false;
            bool isDownward = Random.Range(0, 2) == 0 ? true : false;
            Enemy newEnemy = enemies.CreateEnemy(Enemy.EnemyType.BigGuy, 25, 0.5f, isDownward, isFacingRight, 50);
            newEnemy.transform.position = new Vector3(isFacingRight ? charPos.x - 3f - Random.Range(0f, _spawnDistance - 4f) : charPos.x + 3f + Random.Range(0f, _spawnDistance - 4f), isDownward ? 5.5f : -5.5f, 0);
            bigguyCount += 1;
        }

        #endregion

        #region HitLogic
        foreach (Projectile p in projectiles.List)
        {
            if (p.IsDangerous)
            {
                if ((p.transform.position - charControl.transform.position).magnitude <= 0.3f)
                    if (charControl.Hit(p.Damage))
                    {
                        _nuke.Hurt();
                    }
            }
            else foreach (Enemy e in enemies.List)
                {
                    if (e.gameObject.GetComponent<Enemy>().enabled)
                    {
                        if (laserTime > 0f)
                        {
                            if ((laserDealingRight && e.transform.position.x > 0) || (!laserDealingRight && e.transform.position.x < 0))
                                if (Mathf.Abs(e.transform.position.y - charControl.transform.position.y + 0.2f) < p.HitBoxSize)
                                {
                                    if (e.Hit(8))
                                    {
                                        totalScore += e.Score;
                                    }
                                }
                        }
                        if ((p.transform.position - e.transform.position).magnitude <= 0.8f)
                            if (e.Hit(p.Damage))
                            {
                                totalScore += e.Score;
                            }
                    }

                }
        }

        ResourceManager.Instance.finalScore = totalScore;
        #endregion
    }
}
