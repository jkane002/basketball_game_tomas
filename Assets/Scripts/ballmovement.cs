using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballmovement : MonoBehaviour
{
    public float ballSpeed = 10f;

    /* By default, C# marks all variables as private. The [SerializeField] attribute
     * is used to mark non-public fields as serializable: so that Unity can save and load those values 
     * (all public fields are serialized by default) even though they are not public.
    */ 
    [SerializeField] bool lockCursor = true;
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    float cameraPitch = 0.0f;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        // Lock cursor to the center of the game Window & disable cursor visibility
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        // Separated 2 functions
        UpdateMovement();
        UpdateMouseLook();
    }

    void UpdateMovement() {
        float xDirection = Input.GetAxis("Horizontal"); // x axis
        float zDirection = Input.GetAxis("Vertical"); // z axis

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += ballSpeed * Time.deltaTime * moveDirection;
    }

    void UpdateMouseLook()
    {
        // Created a Vec2 or array of 2 elements - mouseX, mouseY
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
        // ref - pass by reference
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        // Clamp the camera to have 180 degree vision
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(currentMouseDelta.x * mouseSensitivity * Vector3.up);
    }

}
