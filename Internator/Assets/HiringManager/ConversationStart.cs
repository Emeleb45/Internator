using UnityEngine;

public class ConversationStart : MonoBehaviour
{
    public BoxCollider trigger;
    public GameObject dialoguePanel;
    public GameObject promptPanel;
    public GameObject EndPanel;
    void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            promptPanel.SetActive(false);
            return;
        }
        if (EndPanel.activeSelf)
        {
            promptPanel.SetActive(false);
            return;
        }
        if (IsPlayerInsideTrigger())
        {
            promptPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialoguePanel.SetActive(true);
            }
        }
        else
        {
            promptPanel.SetActive(false);
        }
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
