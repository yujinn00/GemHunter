using UnityEngine;
using TMPro; // TextMeshPro Class.

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private float arriveTime = 0.5f;
    private float percent = 0;
    private MovementRigidbody2D movement2D;
    private TextMeshPro text;

    public void Setup(string text, Color color)
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        movement2D.MoveTo(new Vector3(Random.Range(-1f, 1f), 1, 0));

        this.text = GetComponent<TextMeshPro>();
        this.text.text = text;
        this.text.color = color;

        Destroy(gameObject, arriveTime);
    }

    private void Update()
    {
        if (percent > 1) return;

        percent += Time.deltaTime / arriveTime;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - percent);
    }
}
