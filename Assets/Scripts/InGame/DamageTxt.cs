using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamageTxt : MonoBehaviour
{
    Text txt;
    Vector3 endPos;
    Color endColor = new Color(0, 0, 0, 0);
    public IObjectPool<GameObject> Pool { get; set; }
    // Start is called before the first frame update
    public void Setting(Vector3 pos, float damage)
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
        txt = gameObject.GetComponentInChildren<Text>();
        txt.color = Color.red;
        txt.text = damage.ToString();
        endPos = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
        Invoke("Relese", 1.5f);
    }
    void Relese()
    {
        Pool.Release(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endPos, 1f * Time.deltaTime);
        if (txt)
        {
            txt.color = Color.Lerp(txt.color, endColor, 1f * Time.deltaTime);
        }
    }
}
