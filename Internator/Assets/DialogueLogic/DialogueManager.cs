using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson;

    [Header("UI Elements")]
    public GameObject dialoguePanel; 
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;
    public CharacterMovementPOV playerMovement;

    private List<DialogueNode> dialogueNodes;
    private DialogueNode currentNode;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.MovementLocked = true;
        playerMovement.CameraLocked = true;
        LoadDialogue();
        StartDialogue();
    }
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.MovementLocked = false;
        playerMovement.CameraLocked = false;
    }

    void LoadDialogue()
    {
        if (dialogueJson == null)
        {

            return;
        }

        DialogueContainer dialogueData = JsonUtility.FromJson<DialogueContainer>(dialogueJson.text);

        if (dialogueData == null || dialogueData.Lines == null || dialogueData.Lines.Count == 0)
        {

            return;
        }

        dialogueNodes = dialogueData.Lines;

    }

    void StartDialogue()
    {
        if (dialogueNodes.Count > 0)
        {
            ShowNode(dialogueNodes[0]); 
        }
    }

    void ShowNode(DialogueNode node)
    {
        currentNode = node;
        dialogueText.text = node.Text; 
        npcNameText.text = node.Name;  


        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }


        if (node.Choises == null || node.Choises.Count == 0)
        {
            Debug.Log("No choices available. Dialogue might be ending.");
            return;
        }



        foreach (var choice in node.Choises)
        {


            GameObject newButton = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;


            int choiceIndex = node.Choises.IndexOf(choice);
            newButton.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choiceIndex));
        }


    }




    public void ChooseOption(int choiceIndex)
    {
        if (currentNode == null || choiceIndex < 0 || choiceIndex >= currentNode.Choises.Count)
        {
            Debug.LogError("Invalid choice!");
            return;
        }

        int nextId = currentNode.Choises[choiceIndex].Next;


        if (nextId == 0)  
        {

            dialoguePanel.SetActive(false); 
            return;
        }


        DialogueNode nextNode = dialogueNodes.Find(node => node.ID == nextId);

        if (nextNode != null)
        {
            ShowNode(nextNode);
        }
        else
        {
            dialoguePanel.SetActive(false); 
        }
    }


}
