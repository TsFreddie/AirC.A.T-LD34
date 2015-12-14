using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    //public float _boundWidth = 2f; // Move abandaned
    public int _maxHealth = 10;
    public float _boundHeight = 7.6f;
    public float _maxSpeed = 5f;

    public AudioClip _explosion;

    public Animator ufoanim;
    public Animator headAnim;
    public ProgressFilling healthBar;

    public Nuke nuke;

    private Vector3 movingDirection;
    public AudioSource audio;
    private int health = 0;

    private float invisibleTime = 0f;

    void Start () {
        movingDirection = new Vector3(0, 1, 0);
        health = _maxHealth;
        healthBar.UpdateFillAmount(1f);
    }

    bool Left()
    {
        return Input.GetKey(KeyCode.A) || Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.F);
    }

    bool Right()
    {
        return Input.GetKey(KeyCode.D) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.J);
    }

    // nya~
    void Update()
    {
        #region Move - Envolving mouse movement. abandaned
        /*
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        if (worldPos.x < -_boundWidth / 2f)
        {
            worldPos.x = -_boundWidth / 2f;
        }
        if (worldPos.x > _boundWidth / 2f)
        {
            worldPos.x = _boundWidth / 2f;
        }

        Vector3 displacement = worldPos - transform.position;
        displacement.z = 0;
        if (transform.position != worldPos)
            transform.position = transform.position + displacement.normalized * ((displacement.magnitude < Time.deltaTime * _maxSpeed) ?  displacement.magnitude : Time.deltaTime * _maxSpeed);
        */
        #endregion

        #region AutoMove - halt when press any button

        float halfBountHeight = _boundHeight / 2f;
        if (!(Left() || Right()))
        {
            ufoanim.SetBool("stop", false);
            transform.position = transform.position + movingDirection * Time.deltaTime * _maxSpeed;
        }
        else
        {
            ufoanim.SetBool("stop", true);
        }
            
        if (transform.position.y < -halfBountHeight || transform.position.y > halfBountHeight)
        {
            Vector3 currentPos = transform.position;
            currentPos.y = currentPos.y > 0 ? 2 * halfBountHeight - currentPos.y : -2 * halfBountHeight - currentPos.y;

            Debug.Log(string.Format("Character hit bound: {0:f3} -> {1:f3}", transform.position.y, currentPos.y));

            transform.position = currentPos;
            movingDirection = (-movingDirection).normalized;
        }

        #endregion

        #region Invisiblity

        if (invisibleTime > 0) invisibleTime -= Time.deltaTime;

        #endregion
    }

    public float PredictY()
    {
        Vector3 tempPos = transform.position + movingDirection * 8.167f;
        float halfBountHeight = _boundHeight / 2f;
        if (tempPos.y < -halfBountHeight || tempPos.y > halfBountHeight)
        {
            tempPos.y = tempPos.y > 0 ? 2 * halfBountHeight - tempPos.y : -2 * halfBountHeight - tempPos.y;
        }
        return tempPos.y;
    }

    public bool Hit(int damage)
    {
        if (invisibleTime > 0) return false;
        
        health -= damage;
        healthBar.UpdateFillAmount(health / (float)_maxHealth);
        headAnim.SetTrigger("hit");
        invisibleTime = 1f;
        if (health <= 0)
        {
            audio.PlayOneShot(_explosion);
            nuke.Die();
            gameObject.SetActive(false);
        }
        else
            audio.Play();
        return true;

    }
}
