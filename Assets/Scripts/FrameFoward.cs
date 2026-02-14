using UnityEngine;

public class FrameFoward : MonoBehaviour
{
    SharedVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = FindAnyObjectByType<SharedVariables>();
    }

    // Update is called once per frame
    public void Click()
    {
        variables.StepFrames = 1;
    }
}
