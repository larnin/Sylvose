using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Contaminable))]
public class Plant : MonoBehaviour
{
    public Color uncontaminedColor;
    public Color contaminedColor;

    float _height = 0.0f;
    GameObject _tige;
    GameObject _plateform;
    SpriteRenderer _plateformRenderer;
    SpriteRenderer _tigeRenderer;
    Contaminable _contaminable;
    BoxCollider2D _box;

	void Start ()
    {
        _contaminable = GetComponent<Contaminable>();
        _tige = transform.GetChild(1).gameObject;
        _tigeRenderer = _tige.GetComponent<SpriteRenderer>();
        _plateform = transform.GetChild(0).gameObject;
        _plateformRenderer = _plateform.GetComponent<SpriteRenderer>();
        _box = GetComponent<BoxCollider2D>();

        Grow(0);
    }
	
	void Update ()
    {
        Color color = uncontaminedColor * (1 - _contaminable.ContaminationProportion()) + contaminedColor * _contaminable.ContaminationProportion();
        _tigeRenderer.color = color;
        _plateformRenderer.color = color;
    }

    public void Grow(float value)
    {
        _height += value;
        float width = 2 * Mathf.Sqrt(_height);

        _tige.transform.localScale = new Vector3(_tige.transform.localScale.x, _height, _tige.transform.localScale.z);
		_tige.transform.position = new Vector3(_tige.transform.position.x, transform.position.y + _height*1.5f, _tige.transform.position.z);
        _plateform.transform.localScale = new Vector3(width/2, _plateform.transform.localScale.y, _plateform.transform.localScale.z);
        _plateform.transform.position = transform.position + new Vector3(0, _height*3, 0);
		_box.size = new Vector2(width+0.05f, _height+0.05f);
        _box.offset = new Vector2(0, _box.size.y / 2);
    }
}
