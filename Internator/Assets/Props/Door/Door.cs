using UnityEngine;
using TMPro;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = -110.0f;
    public float closeAngle = 0.0f;
    public float smooth = 2.0f;
    public GameObject door;
    public BoxCollider trigger;

    private Collider doorCollider;
    private bool isPlayerInsideTrigger = false;
    private Quaternion targetRotation;
    public GameObject promptPanel;
    void Start()
    {
        doorCollider = door.GetComponent<Collider>();
        targetRotation = door.transform.localRotation;
    }

    public void OpenDoor()
    {


        if (isOpen)
        {
            targetRotation = Quaternion.Euler(0, closeAngle, 0);
            doorCollider.enabled = true;
        }
        else
        {
            targetRotation = Quaternion.Euler(0, openAngle, 0);
            doorCollider.enabled = false;
        }

        isOpen = !isOpen; 
    }

    void Update()
    {

        if (IsPlayerInsideTrigger())
        {
            promptPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenDoor();
            }
        }
        else
        {
            promptPanel.SetActive(false);
        }
        door.transform.localRotation = Quaternion.RotateTowards(door.transform.localRotation, targetRotation, smooth * Time.deltaTime * 100);
    }


    bool IsPlayerInsideTrigger()
    {
        Collider[] colliders = Physics.OverlapBox(trigger.bounds.center, trigger.bounds.extents, Quaternion.identity);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

}

//door.transform.localRotation = Quaternion.RotateTowards(door.transform.localRotation, targetRotation, smooth * Time.deltaTime * 100);