using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 250f;
    [SerializeField] float thrustSpeed = 40f;

    Rigidbody rb;
    AudioSource flySound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        flySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("VICTORY!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // Loads the next level
                break;
            default:
                print("DEAD");
                SceneManager.LoadScene(0);  // Returns to level 1
                break;
        }
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
            if (flySound.isPlaying == false)
                flySound.Play();
        }
        else
        {
            flySound.Stop();
        }
    }
}
