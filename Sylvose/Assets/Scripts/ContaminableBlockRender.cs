using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Contaminable), typeof(SpriteRenderer))]
public class ContaminableBlockRender : MonoBehaviour
{
    public Color uncontaminedColor;
    public Color contaminedColor;

    Contaminable _contaminable;
    SpriteRenderer _renderer;

	void Start ()
    {
        _contaminable = GetComponent<Contaminable>();
        _renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        _renderer.color = uncontaminedColor * (1-_contaminable.ContaminationProportion()) + contaminedColor * _contaminable.ContaminationProportion();
	}
}
