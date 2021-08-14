using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    Vector3 CameraTarget;
    GameObject PLayer;

   [SerializeField] float CameraTargetOffset_Y;
   [SerializeField] float Distance_Z;

    float Rotation_X;
    float Rotation_Y;
    

    const float Min_X = -15.0f;
    const float Max_X =  15.0f;


    const float Min_Z = 1.76f;
    const float Max_Z =  8.0f;

  

    // Start is called before the first frame update
    void Start()
    {
        PLayer = GameObject.FindGameObjectWithTag("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        CameraTarget = PLayer.transform.position;
        CameraTarget.y += CameraTargetOffset_Y;

        if (Input.GetButton("Fire2"))
        {
            Rotation_Y += Input.GetAxis("Mouse X");
            Rotation_X += Input.GetAxis("Mouse Y");

          Rotation_X =  Mathf.Clamp(Rotation_X, Min_X, Max_X);
        }

        Distance_Z -= Input.GetAxis("Mouse ScrollWheel");
        Distance_Z = Mathf.Clamp(Distance_Z, Min_Z, Max_Z);

    


    }

    private void LateUpdate()
    {
        Quaternion CameraRotation = Quaternion.Euler(Rotation_X, Rotation_Y, 0);
        Vector3 CameraOffset = new Vector3(0, 0, -Distance_Z);


        transform.position = CameraTarget + CameraRotation * CameraOffset;
        transform.LookAt(CameraTarget);

       
    }

    public Vector3 GetForwardVector()
    {
        Quaternion rotation = Quaternion.Euler(0, Rotation_Y, 0);
        return rotation * Vector3.forward;
    }
}
