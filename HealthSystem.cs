using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region HP

    public float fHp;
    float hp;

    public float invForce;
    public float invT;
    float invF;

    bool invAnim;
    float invAnimT;
    float invAnimTF; // full

    bool Dmger;

    #endregion

    #region Others

    private BoxCollider2D bc; // game object collision
    private Rigidbody2D rb; // game object rigid body ( psysics )
    private GameObject OtherGM;

    #endregion

    // Awake is called before the first frame update
    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        hp = fHp;

        invForce = 11f;
        invF = .75f;
        invT = 0;
        invAnimTF = .2f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AutoUpdateVariables();
        Health();
        MyDebug();
    }

    private void MyDebug()
    {
    }

    private void Health()
    {
        if (Dmger && invT <= 0)
        {
            LoseHealth();
        }

        InvulnerabilityAnimation();

        if (hp <= 0)
        {
            Death();
        }
    }

    public void LoseHealth()
    {
        hp -= 1;
        invT = invF;
        OtherGM.GetComponent<Rigidbody2D>().velocity = new Vector2(OtherGM.GetComponent<Rigidbody2D>().velocity.x, invForce);
        rb.velocity = new Vector2(invForce * -transform.localScale.x, invForce);
    }

    public void InvulnerabilityAnimation()
    {
        if (invT >= 0)
        {
            invT -= Time.deltaTime;
            invAnimT -= (Time.deltaTime * 4f);
            if (invAnim && invAnimT <= 0)
            {
                invAnim = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                invAnimT = invAnimTF;
            }
            else if (!invAnim && invAnimT <= 0)
            {
                invAnim = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                invAnimT = invAnimTF;
            }
        }
        else
        {
            invAnim = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }

    private void AutoUpdateVariables()
    {
        if (gameObject.tag == "Player")
        {
            Dmger = Physics2D.OverlapBox(new Vector2(bc.bounds.center.x, bc.bounds.center.y + .1f), new Vector2(bc.size.x, bc.size.y - .5f), 0f, LayerMask.GetMask("Enemy"));
            if (GameObject.FindWithTag("Enemy"))
            {
                OtherGM = GameObject.FindWithTag("Enemy");
            }
        }
        else if (gameObject.tag == "Enemy")
        {
            Dmger = Physics2D.OverlapBox(new Vector2(bc.bounds.center.x, bc.bounds.center.y + .1f), new Vector2(bc.size.x - .05f, bc.size.y - .15f), 0f, LayerMask.GetMask("Player"));
            if (GameObject.FindWithTag("Player"))
            {
                OtherGM = GameObject.FindWithTag("Player");
            }
        }
    }
}
