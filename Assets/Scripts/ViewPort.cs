using UnityEngine;

public class ViewPort : MonoBehaviour
{
    UpdateLoop updateLoop;
    SharedVariables variables;
    Vector2 ViewPortSize;
    RectTransform rectTransform;
    int width;
    int height;
    float y;
    float x;
    float WToHRatio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = GetComponent<SharedVariables>();
        updateLoop = GetComponent<UpdateLoop>();

        rectTransform = GetComponent<RectTransform>();
        width = variables.Width;
        height = variables.Height;
        WToHRatio = (float)width / (float)height; // X/Y

        x = rectTransform.rect.width;
        y = rectTransform.rect.height;
        setViewPortSize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setViewPortSize()
    {

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
