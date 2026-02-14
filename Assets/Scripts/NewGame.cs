using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    [SerializeField] GameObject Options;
    [SerializeField] TMP_InputField HeightText;
    [SerializeField] TMP_InputField WidthText;
    SharedVariables variables;
    ViewPort viewPort;
    UpdateLoop updateLoop;

    void Start()
    {
        variables = FindFirstObjectByType<SharedVariables>();
        viewPort = FindFirstObjectByType<ViewPort>();
        updateLoop = FindFirstObjectByType<UpdateLoop>();
    }
    public void Show()
    {
        Options.SetActive(true);
        HeightText.text = variables.Height.ToString();
        WidthText.text = variables.Width.ToString();
        variables.CanPlayerInput = false;
    }

    public void Confirm()
    {
        int height;
        int width;
        if (int.TryParse(HeightText.text, out height) == false)
        {
            print("Can't parse canceling");
            Show();
            return;
        }
        if (int.TryParse(WidthText.text, out width) == false)
        {
            print("Can't parse canceling");
            Show();
            return;
        }

        print("Variables Parsed");
        variables.Height = height;
        variables.Width = width;
        variables.InitVariables();

        viewPort.setViewPortSize();
        updateLoop.Init();

        Close();


    }
    public void Close()
    {
        Options.SetActive(false);
        variables.CanPlayerInput = true;
    }
}
