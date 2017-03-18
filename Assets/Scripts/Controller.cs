using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public float MaxSpeed = 10.0f;
    public float JumpModifier = 5.0f;
    private float horizontalInput;
    private Rigidbody2D rigidbody2D;
    private bool jumpButtonPressed;
	public LayerMask groundLayer;
	private Transform groundSensor;
	public  AudioSource marshmallowSound;
    public AudioSource deathSound;
    public AudioSource jumpSound;
    public AudioSource leveDone;

    // Use this for initialization
    private void Start ()
	{
	    this.rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
		this.groundSensor = this.gameObject.transform.FindChild("GroundSensor");
	
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    this.horizontalInput = Input.GetAxis("Horizontal");

	    this.jumpButtonPressed = Input.GetButtonDown("Jump");
	}

    //FixedUpdate is called every fixed framerate frame
    private void FixedUpdate()
    {
        var speed = this.MaxSpeed * this.horizontalInput;
        var moveSpeed = new Vector2(speed, this.rigidbody2D.velocity.y);
        this.rigidbody2D.velocity = moveSpeed;
		var grounded = Physics2D.OverlapCircle(groundSensor.transform.position, 0.15f, groundLayer) != null;
		//Debug.Log(grounded);

		if(this.jumpButtonPressed && grounded )
        {
            jumpSound.Play();
            var jumpForce = new Vector2(0, this.JumpModifier);

            this.rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
			this.jumpButtonPressed = false;
        }
    }


	void OnTriggerEnter2D (Collider2D collision)
	{
		if(collision.gameObject.name == "growMarshmallow")
		{
			marshmallowSound.Play ();
			Destroy(collision.gameObject);
            biggerPlayerAndIncreaseJumpHeight();
		}

        if (collision.gameObject.name == "shrinkMarshmallow")
        {
            marshmallowSound.Play();
            Destroy(collision.gameObject);
            smallerPlayerAndDecreaseJumpHeight();
        }

        if (collision.gameObject.name == "Items-spikes")
        {
            deathSound.Play();
            SceneManager.LoadScene(1);
        }

        if (collision.gameObject.name == "deathLine")
        {
            deathSound.Play();
            SceneManager.LoadScene(1);
        }

        if (collision.gameObject.name == "Items-cactus")
        {
            deathSound.Play();
            SceneManager.LoadScene(2);
        }

        Debug.Log("name " + collision.gameObject.name);
	}

	void biggerPlayerAndIncreaseJumpHeight(){
        float scaleSize = 0.6F;
        this.transform.localScale += new Vector3(scaleSize, scaleSize, 0);
        float force = 7.0f;
        JumpModifier = force;
    }

    void smallerPlayerAndDecreaseJumpHeight()
    {
        float scaleSize = -0.6F;
        this.transform.localScale += new Vector3(scaleSize, scaleSize, 0);
        float force = 5.0f;
        JumpModifier = force;
    }


}
