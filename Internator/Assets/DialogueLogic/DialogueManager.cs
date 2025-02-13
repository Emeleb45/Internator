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
    public Transform playerTransform;
    public GameObject playerCam;
    public GameObject talkCam;
    public Animator playerAnimator;

    private List<DialogueNode> dialogueNodes;
    private DialogueNode currentNode;

    void OnEnable()
    {
        if (playerMovement.TipsOn)
        {
            playerMovement.ToggleTips();
        }
        playerMovement.Speaking = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerCam.SetActive(false);
        talkCam.SetActive(true);
        playerAnimator.SetLayerWeight(1, 1f);
        playerMovement.MovementLocked = true;
        playerMovement.CameraLocked = true;
        playerTransform.position = new Vector3(0, 0.5f, 7);
        playerTransform.rotation = Quaternion.Euler(0, 0, 0);
        LoadDialogue();
        StartDialogue();
    }
    void OnDisable()
    {
        if (playerMovement.EndScreen.activeSelf == false)
        {
            playerMovement.Speaking = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerAnimator.SetLayerWeight(1, 0f);
            talkCam.SetActive(false);
            playerCam.SetActive(true);
            playerMovement.MovementLocked = false;
            playerMovement.CameraLocked = false;
        }

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
        playerAnimator.SetTrigger("Sitting Talking");

        if (currentNode == null || choiceIndex < 0 || choiceIndex >= currentNode.Choises.Count)
        {
            Debug.LogError("Invalid choice!");
            return;
        }


        DialogueChoice choice = currentNode.Choises[choiceIndex];


        if (choice.Functions != null)
        {
            foreach (DialogueFunction function in choice.Functions)
            {
                if (function.Name == "UpdatePoints" && function.Params.Count > 0)
                {
                    float amount;
                    if (float.TryParse(function.Params[0], out amount))
                    {
                        UpdatePoints(amount);
                    }
                }
                else if (function.Name == "PlayAnimation" && function.Params.Count > 0)
                {
                    PlayAnimation(function.Params[0]);
                }
            }
        }
        int nextId = choice.Next;

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

    void PlayAnimation(string AnimationName)
    {
        playerAnimator.SetTrigger(AnimationName);
    }

    void UpdatePoints(float amnt)
    {
        playerMovement.UpdatePoints(amnt);
    }
}
