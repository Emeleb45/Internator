using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueContainer
{
    public List<DialogueNode> Lines;
}

[System.Serializable]
public class DialogueNode
{
    public int ID;
    public string Name;
    public string Text;
    public List<DialogueChoice> Choises;
}
[System.Serializable]
public class DialogueFunction
{
    public string Name;
    public List<string> Params;
}
[System.Serializable]
public class DialogueChoice
{
    public string Text;
    public int Next;
    public List<DialogueFunction> Functions;
}
