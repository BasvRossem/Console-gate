using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    public float blinkingSpeed;
    private float lastBlinkUpdate;

    Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(0, 0);
        lastBlinkUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastBlinkUpdate > blinkingSpeed)
        {
            GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
            lastBlinkUpdate = Time.time;
        }
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
