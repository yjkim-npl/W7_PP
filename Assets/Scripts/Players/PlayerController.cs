using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curInput;
    public float jumpPower;
    public LayerMask maskForJumpReset;

    private Rigidbody rb;

    [Header("Look")]
    public Transform camContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [Header("Inventory")]
    private bool canLook = true;
    public GameObject crossHair;
    public Action Inventory;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if(canLook)
        {
            CamLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curInput.y + 
                      transform.right   * curInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curInput = Vector2.zero;
        }
    }
    private void CamLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        camContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
        Debug.Log("Jump method was invoked");
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    private bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f - transform.up * 0.3f,Vector3.down),
            new Ray(transform.position - transform.forward * 0.2f - transform.up * 0.3f,Vector3.down),
            new Ray(transform.position + transform.right   * 0.2f - transform.up * 0.3f,Vector3.down),
            new Ray(transform.position - transform.right   * 0.2f - transform.up * 0.3f,Vector3.down)
        };
        foreach(Ray ray in rays)
        {
            Debug.DrawRay(ray.origin,ray.direction,Color.red);
            if (Physics.Raycast(ray,1f,maskForJumpReset))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Inventory?.Invoke();
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle?CursorLockMode.None:CursorLockMode.Locked;
        crossHair.SetActive(!crossHair.activeSelf);
        canLook = !toggle;
    }
}