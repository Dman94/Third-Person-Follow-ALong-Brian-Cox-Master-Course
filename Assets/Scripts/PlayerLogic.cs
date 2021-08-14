using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{

    float Horinput, VerInput;

   [SerializeField] float MoveSpeed;
   [SerializeField]  bool Jump = false;
   [SerializeField] float JumpHeight = 0.4f;
   [SerializeField] float Gravity = 0.0045f;

    Vector3 HeightMovement;
    Vector3 HorizontalMovement;
    Vector3 VerticalMovement;


    CharacterController characterController;
    GameObject camera;
    CameraLogic cameraLogic;

    Animator animator;
    [SerializeField] List<AudioClip> EarthFootSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> StoneFootSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> PuddleFootSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> GrassFootSteps = new List<AudioClip>();
    AudioSource audioSource;

    [SerializeField] Transform LeftToeBase;
    [SerializeField] Transform RightToeBase;
   
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        camera = Camera.main.gameObject;

        if (camera)
        {
            cameraLogic = camera.GetComponent<CameraLogic>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        Horinput = Input.GetAxis("Horizontal");
        VerInput = Input.GetAxis("Vertical");

       

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            Jump = true;
        }

        if (animator)
        {
            animator.SetFloat("Vertical Input", VerInput);
            animator.SetFloat("Horizontal Input", Horinput);
        }
       
    }

     void FixedUpdate()
    {
        if (Jump)
        {
            HeightMovement.y = JumpHeight;
            Jump = false;
        }

        if (cameraLogic && (Mathf.Abs(Horinput) > 0.1f || Mathf.Abs(VerInput) > 0.1f))
        {
            transform.forward = cameraLogic.GetForwardVector();
        }

        HeightMovement.y -= Gravity * Time.deltaTime;
        VerticalMovement = transform.forward * VerInput * MoveSpeed * Time.deltaTime;
        HorizontalMovement = transform.right * Horinput * MoveSpeed * Time.deltaTime;

        characterController.Move(HorizontalMovement + HeightMovement + VerticalMovement);



        if (characterController.isGrounded)
        {
            HeightMovement.y = 0.0f;
        }


    }

    void PlayFootStepSound(int FootStepsIndex)
    {
        if (FootStepsIndex == 0)
        {
            RayCastToTerrain(LeftToeBase.position);
        }
        else if (FootStepsIndex == 1)
        {
            RayCastToTerrain(RightToeBase.position);
           
        }
     

    }

     void PlayRandomSound(List<AudioClip> audioClips)
    {
        if (audioClips.Count > 0 && audioSource)
        {
            int randomNum = Random.Range(0, audioClips.Count );

            audioSource.PlayOneShot(audioClips[randomNum]);
        }
    }

    void RayCastToTerrain(Vector3 Position)
    {
        LayerMask layermask = LayerMask.GetMask("Terrain");
    
        Ray ray = new Ray(Position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layermask))
        {
           string hitTag = hit.collider.gameObject.tag;

            if (hitTag == "Earth")
            {
                PlayRandomSound(EarthFootSteps);
            }
            else if (hitTag == "Stone")
            {
                PlayRandomSound(StoneFootSteps);
            }
            else if (hitTag == "Grass")
            {
                PlayRandomSound(GrassFootSteps);
            }
            else if (hitTag == "Puddle")
            {
                PlayRandomSound(PuddleFootSteps);
            }
        }

    }
}
