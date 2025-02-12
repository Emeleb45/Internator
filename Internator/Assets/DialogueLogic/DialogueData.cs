using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueContainer
{
    public List<DialogueNode> Lines; // Matches JSON key "Lines"
}

[System.Serializable]
public class DialogueNode
{
    public int ID; // Matches "ID"
    public string Name; // Matches "Name"
    public string Text; // Matches "Text"
    public List<DialogueChoice> Choises; // Matches "Answers" in JSON
}

[System.Serializable]
public class DialogueChoice
{
    public string Text; // Matches "Text"
    public int Next; // Matches "Next"
}
