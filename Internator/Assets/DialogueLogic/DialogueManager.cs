using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson;

    [Header("UI Elements")]
    public GameObject dialoguePanel; // The dialogue UI panel
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    private List<DialogueNode> dialogueNodes;
    private DialogueNode currentNode;

    void Start()
    {
        LoadDialogue();
        StartDialogue();
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
            ShowNode(dialogueNodes[0]); // Start at the first node
        }
    }

    void ShowNode(DialogueNode node)
    {
        currentNode = node;
        dialogueText.text = node.Text; // Update dialogue text
        npcNameText.text = node.Name;  // Update NPC name

        // ❌ Clear previous buttons (to avoid duplicate choices)
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // ✅ Check if the node has Choises
        if (node.Choises == null || node.Choises.Count == 0)
        {
            Debug.Log("No choices available. Dialogue might be ending.");
            return;
        }


        // ✅ Generate choice buttons for the new dialogue node
        foreach (var choice in node.Choises)
        {


            GameObject newButton = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;

            // ✅ Ensure button calls ChooseOption() with the correct index
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

        // If there is no valid next ID, end the dialogue
        if (nextId == 0)  // You can also check if it's -1 or any "end" indicator
        {

            dialoguePanel.SetActive(false); // Hide dialogue UI
            return;
        }

        // Find the next node by its ID
        DialogueNode nextNode = dialogueNodes.Find(node => node.ID == nextId);

        if (nextNode != null)
        {
            ShowNode(nextNode);
        }
        else
        {
            dialoguePanel.SetActive(false); // Hide dialogue UI
        }
    }


}
