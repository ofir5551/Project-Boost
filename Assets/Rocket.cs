using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 250f;
    [SerializeField] float thrustSpeed = 40f;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip LevelUPSound;
    [SerializeField] AudioClip DeathSound;

    [SerializeField] ParticleSystem ThurstParticles;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem DeathParticles;

    Rigidbody rb;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }       // Ignores collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartLevelUpSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartLevelUpSequence()
    {
        SuccessParticles.Play();
        state = State.Transcending;
        
        print("Transcending to next level (" + (SceneManager.GetActiveScene().buildIndex + 2) + ")......");
        Invoke("LoadNextScene", 2f);   // Loads the next level
        audioSource.Stop();
        audioSource.PlayOneShot(LevelUPSound);
    }

    private void StartDeathSequence()
    {
        DeathParticles.Play();
        state = State.Dying;

        print("DEAD");
        Invoke("LoadFirstLevel", 2f);  // Returns to level 1
        audioSource.Stop();
        audioSource.PlayOneShot(DeathSound);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        state = State.Alive;
        print("Level " + (SceneManager.GetActiveScene().buildIndex + 2) + " successfully loaded.");
    }

    private void Rotation()
    {
        rb.freezeRotation = true;
        float speed = rotateSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * speed);
        }

        rb.freezeRotation = false;
    }

    private void Thrust()
    {
        // float tSpeed = thrustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustSpeed);
            ThurstParticles.Play();
            if (audioSource.isPlaying == false)
                audioSource.PlayOneShot(MainEngine);
        }
        else
        {
            audioSource.Stop();
            ThurstParticles.Stop();
        }
    }
}
