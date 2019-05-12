using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 250f;
    [SerializeField] float thrustSpeed = 1800f;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip LevelUPSound;
    [SerializeField] AudioClip DeathSound;

    [SerializeField] ParticleSystem ThurstParticles;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem DeathParticles;

    Rigidbody rb;
    AudioSource audioSource;
    static int lives = 3;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (lives == 0)
            lives = 3;
        print("Level " + (SceneManager.GetActiveScene().buildIndex + 1) + " successfully loaded.");
        print("Lives: " + lives);
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
        if (lives == 1)
        {
            print("GAME OVER");
            Invoke("LoadFirstLevel", 2f);  // Returns to level 1
            
        }
        else
            Invoke("LoadCurrentLevel", 2f);  // Returns to level 1

        audioSource.Stop();
        audioSource.PlayOneShot(DeathSound);
        lives -= 1;
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        state = State.Alive;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        state = State.Alive;
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
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);
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
