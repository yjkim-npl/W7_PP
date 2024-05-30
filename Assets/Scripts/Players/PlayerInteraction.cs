using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.5f;
    private float lastCheckTime;
    public float checkDistance;
    public LayerMask mask;

    public GameObject curInteractGO;
    private IInteractable curInteractable;

    public GameObject textParentGO;
    public TextMeshProUGUI promptTxt;
    private Camera cam; // for ray
    void Start()
    {
        cam = Camera.main; 
        textParentGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, checkDistance, mask))
            {
                if(hit.collider.gameObject != curInteractGO)
                {
                    curInteractGO = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGO = null;
                curInteractable = null;
                textParentGO.SetActive(false);
            }
        }
    }
    private void SetPromptText()
    {
        textParentGO.SetActive(true);
        promptTxt.text = curInteractable.GetInteractPrompt();
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGO = null;
            curInteractable = null;
            textParentGO.SetActive(false);
        }
    }
}
