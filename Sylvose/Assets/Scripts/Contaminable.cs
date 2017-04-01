using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contaminable : MonoBehaviour
{
    [Tooltip("Le rayon de propagation lorsque cet élément est contaminé.")]
    public float contaminationRadius = 1.0f;
    [Tooltip("La puissance de contamination lorsque cet élément est contaminé.")]
    public float spreadPower = 1.0f;
    [Range(0.01f, 100.0f)]
    [Tooltip("La résistance de cet élément à la contamination (doubler cette valeur double la résistance).")]
    public float contaminationResistance = 1.0f;
    [Tooltip("Le niveau de contamination de l'objet à son instantiation (entre 0 et 100)")]
    public float contaminationLevel = 0;

    protected Coroutine _contaminationCoroutine;

    public enum ContaminationLevel
    {
        UNCONTAMINED = 0,
        SMALL = 20,
        MEDIUM = 45,
        HIGHT = 70,
        CONTAMINED = 100
    }

    protected virtual void Start()
    {
        if(IsContamined() == ContaminationLevel.CONTAMINED)
        {
            contaminationLevel = (int)ContaminationLevel.CONTAMINED;
            _contaminationCoroutine = StartCoroutine(ContamineCoroutine());
        }
    }

    public void Contamine(float value)
    {
        if (IsContamined() == ContaminationLevel.CONTAMINED && Mathf.Sign(value) > 0)
            return;

            contaminationLevel += value / contaminationResistance;
        if(IsContamined() == ContaminationLevel.CONTAMINED)
        {
            contaminationLevel = (int)ContaminationLevel.CONTAMINED;
            if(_contaminationCoroutine == null)
                _contaminationCoroutine = StartCoroutine(ContamineCoroutine());
        }
    }

    public ContaminationLevel IsContamined()
    {
        ContaminationLevel level = ContaminationLevel.UNCONTAMINED;

        foreach (ContaminationLevel value in Enum.GetValues(typeof(ContaminationLevel)))
        {
            if (value > level && (int)value <= contaminationLevel)
                level = value;
        }

        return level;
    }

    public float ContaminationProportion()
    {
        return contaminationLevel / ((int)ContaminationLevel.CONTAMINED - (int)ContaminationLevel.UNCONTAMINED);
    }

    protected virtual IEnumerator ContamineCoroutine()
    {
        while (IsContamined() == ContaminationLevel.CONTAMINED)
        {
            float value = spreadPower * Time.deltaTime;
            var objects = Physics2D.OverlapCircleAll(transform.position, contaminationRadius);
            foreach(var o in objects)
            {
                var contamineCompent = o.gameObject.GetComponent<Contaminable>();
                if (contamineCompent == null)
                    continue;
                contamineCompent.Contamine(value);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
