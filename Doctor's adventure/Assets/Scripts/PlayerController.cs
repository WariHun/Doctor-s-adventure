using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; //Mozgási sebesség
    public float jumpHeight;    //Ugrási magasság
    public float lowGrav = 2f;  //A gravitáció manipulálására változó
    public float maxFalling = 10f;  //Max esési sebesség
    private float canJump = 0f; //Timer az ugrás cooldown-jára
    private bool facingRight;   //Változó hogy melyik irányba néz
    private Rigidbody rb;   //Saját rigidbody
    private Animator anim;  //Animátor

    void Awake()
    {
        //A rigidbody felvétele, és a karakter nézési irányának megadása (kezdéskor jobbra néz)
        rb = GetComponent<Rigidbody>();
        facingRight = true;
        if (LoadMenu.saveslot != 0)
        {
            SaveLoadScript.Load(LoadMenu.saveslot);   //Mentés betöltése (ha kell)
            LoadMenu.saveslot = 0;
        }
        else if (LoadMenu.saveslot == 0) DialogueManager.prologue = false;  //Új játék kezdésénél, legyen prológus
    }

    void Start()
    {
        anim = transform.Find("Doctor").gameObject.GetComponent<Animator>();    //Az animátor megkeresése
    }

    void Update()
    {
        if (!DialogueManager.prologue) return;  //Amig a dialógus megy, ne lehessen használni a különböző billentyűket

        //Animátor állítása
        anim.SetFloat("vSpeed", rb.velocity.y);
        anim.SetFloat("hSpeed", Mathf.Abs(Input.GetAxis("Horizontal")));

        //Y tengeren való mozgásra animátor és grounded változó állítása
        if (Mathf.Abs(rb.velocity.y) > 0.5f)
        {
            anim.SetBool("onGround", false);
            if (rb.velocity.y < 0.5) GroundChecker.grounded = false;
        }
        else anim.SetBool("onGround", GroundChecker.grounded);

        //Ugrás
        if (Input.GetAxis("Jump") > 0 && GroundChecker.grounded && Time.time > canJump)
        {
            GroundChecker.grounded = false;
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            canJump = Time.time + 1f;
        }

        //Esés manipulálása
        if (rb.velocity.y < -0.2f) rb.velocity += Vector3.up * Physics.gravity.y * (lowGrav - 0.5f) * Time.deltaTime;
        else if (rb.velocity.y > 0.2f && Input.GetAxis("Jump") == 0) rb.velocity += Vector3.up * Physics.gravity.y * (lowGrav - 1f) * Time.deltaTime;

        if (rb.velocity.magnitude>maxFalling) rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxFalling);    //Max esési sebesség

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, RotationManager.degree, 0), 8 * Time.deltaTime);  //A karakter vizuális forgatása

    }
    void FixedUpdate()
    {
        if (!DialogueManager.prologue) return;

        MoveCharacter(Input.GetAxis("Horizontal")); //A karakter mozgatásának meghívása

        //Sprite tükrözés, hogyha szükséges
        if (Input.GetAxis("Horizontal") > 0 && !facingRight) Flip();
        else if (Input.GetAxis("Horizontal") < 0 && facingRight) Flip();
    }

    /// <summary>
    /// A karakter mozgatása
    /// </summary>
    private void MoveCharacter(float moveFactor)
    {
        Vector3 movement = Vector3.zero;
        if (RotationManager.facingDirection == FacingDirection.Front) movement = new Vector3(moveFactor * speed, rb.velocity.y, 0);
        else if (RotationManager.facingDirection == FacingDirection.Right) movement = new Vector3(0, rb.velocity.y, moveFactor * speed);
        else if (RotationManager.facingDirection == FacingDirection.Back) movement = new Vector3(-(moveFactor * speed), rb.velocity.y, 0);
        else if (RotationManager.facingDirection == FacingDirection.Left) movement = new Vector3(0, rb.velocity.y, -(moveFactor * speed));
        rb.velocity = movement;
    }

    /// <summary>
    /// A sprite tükrözése
    /// </summary>
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}