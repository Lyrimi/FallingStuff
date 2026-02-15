using System.Collections.Generic;
using UnityEngine;

public class SelectElement : MonoBehaviour
{
    SharedVariables variables;
    Dictionary<string, Elements.Cell> ElementLookup;

    void Start()
    {
        variables = FindFirstObjectByType<SharedVariables>();
        ElementLookup = new Dictionary<string, Elements.Cell>{
            {"Sand",Elements.sand},
            {"Wood",Elements.wood},
            {"Water",Elements.water}
        };
    }

    public void Click(GameObject gameObject)
    {
        string Name = gameObject.name;
        if (ElementLookup.ContainsKey(Name) == false)
        {
            Debug.LogError("Failed to find key in element lookup");
            return;
        }

        variables.SelectedElement = ElementLookup[Name];
    }
}
