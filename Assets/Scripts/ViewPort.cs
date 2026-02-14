using UnityEngine;

public class ViewPort : MonoBehaviour
{
    UpdateLoop updateLoop;
    SharedVariables variables;
    Vector2 ViewPortSize;
    int width;
    int height;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = GetComponent<SharedVariables>();
        updateLoop = GetComponent<UpdateLoop>();
        setViewPortSize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setViewPortSize()
    {
        width = variables.Width;
        height = variables.Height;
        float WToHRatio = (float)width / (float)height; // X/Y
        RectTransform rectTransform = GetComponent<RectTransform>();
        float x = rectTransform.rect.width;
        float y = rectTransform.rect.height;
        if (height >= width)
        {
            ViewPortSize = new Vector2(y * WToHRatio, y);
        }
        else
        {
            ViewPortSize = new Vector2(x, x / WToHRatio);
        }
        rectTransform.sizeDelta = ViewPortSize;
        variables.ViewPortSize = ViewPortSize;
    }

}
