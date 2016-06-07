using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;
    public float rotateSpeed = 0.1f;
    public float padding = 1f;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 350f;
    public float maxHealth = 350f;
    public AudioClip fireSound;
    public AudioClip deathSound;
    public GameObject explosion;
    public GameObject healthBar;
    public GameObject hitEffect;

    private float xMin;
    private float xMax;
    private SpriteRenderer image;
    private PolygonCollider2D collider;
    private bool isDead = false;
    private Color hitEffectColor;

    // Use this for initialization
    void Start () {
        image = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isDead)
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && !isDead)
        {
            CancelInvoke("Fire");
        }

	    if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !isDead)
        {
            //transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.left * speed * Time.deltaTime;

            /*
             Rotation on sprites does not look that good ^^
            float rotation = Mathf.Clamp(Vector3.right.x * rotateSpeed, 0f, 0.35f);
            if (transform.localRotation.y < 0.35f)
            {
                transform.Rotate(transform.localRotation.x, (rotation + rotateSpeed), transform.rotation.z);
            }*/
           
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !isDead)
        {
            //transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.right * speed * Time.deltaTime;

            /*
             Rotation on sprites does not look that good ^^
            float rotation = Mathf.Clamp(Vector3.left.x * rotateSpeed, 0f, -0.35f);
            print(transform.localRotation.y);
            if (transform.localRotation.y > -0.35f)
            {
                transform.Rotate(transform.localRotation.x, (rotation - rotateSpeed), transform.rotation.z);
            }
            */
        }

        // Restrict the player to game space
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            if (health > 0)
            {
                health -= missile.GetDamage();

                Hit(collider);
                SetHealthBar();
                missile.Hit();
            }
            
            if (health <= 0 && image.enabled)
            {
                image.enabled = false;
                collider.enabled = false;
                isDead = true;
                AudioSource.PlayClipAtPoint(deathSound, transform.position);

                explosion = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(explosion, 2f);

                Invoke("Die", 2f);
            }
        }
    }

    void Hit(Collider2D collider)
    {
        if (hitEffect && health > 0) {
            hitEffect = Instantiate(hitEffect, collider.transform.position, Quaternion.identity) as GameObject;
            hitEffectColor = collider.gameObject.GetComponent<SpriteRenderer>().color;
            hitEffect.GetComponent<ParticleSystem>().startColor = hitEffectColor;
        }
    }

    void SetHealthBar()
    {
        float healthScale = health / maxHealth;
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(healthScale, 0, 1), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    public void AddHealth (int addHealth)
    {
        health += addHealth;
        SetHealthBar();
    }

    void Die()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("LoseScreen");
        Destroy(gameObject);
    }

    void Fire()
    {
        if (!isDead)
        {
            GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
            beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, projectileSpeed, 0f);
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
    }
}
