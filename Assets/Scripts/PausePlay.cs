using UnityEngine;

public class PausePlay : MonoBehaviour
{
    SharedVariables variables;
    public GameObject Pause;
    public GameObject Play;
    void Start()
    {
        variables = FindFirstObjectByType<SharedVariables>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Click()
    {
        if (variables.Paused)
        {
            variables.Paused = false;
            Pause.SetActive(true);
            Play.SetActive(false);
        }
        else
        {
            variables.Paused = true;
            Pause.SetActive(false);
            Play.SetActive(true);
        }
    }
}
