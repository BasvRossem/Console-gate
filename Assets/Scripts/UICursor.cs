using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    public float blinkingSpeed;
    private float lastBlinkUpdate;

    public bool isVisible;

    Vector2 position;
    Vector2 characterSize;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(0, 0);
        characterSize = GetSize();
        isVisible = true;

        lastBlinkUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible && Time.time - lastBlinkUpdate > blinkingSpeed)
        {
            GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
            lastBlinkUpdate = Time.time;
        }
    }

    public void SetVisible(bool onOff)
    {
        GetComponent<Image>().enabled = onOff;
        isVisible = onOff;
    }



    public Vector2 GetSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
    }

    public void SetSize(Vector2 newSize)
    {
        GetComponent<RectTransform>().sizeDelta = newSize;
    }

    public void ResetSize()
    {
        SetSize(characterSize);
    }
    public void SetPositionCenter(Vector2 position)
    {
        transform.position = position;
    }

    public void SetPositionTopLeft(Vector2 position)
    {
        var rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;
        Vector3 centerOffset = new Vector3(width / 2, -height / 2, 0);

        transform.position = position;
        transform.position += centerOffset;
    }
}
