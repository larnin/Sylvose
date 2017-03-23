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

    float _contaminationLevel = 0;

    public enum ContaminationLevel
    {
        UNCONTAMINED = 0,
        SMALL = 20,
        MEDIUM = 45,
        HIGHT = 70,
        CONTAMINED = 100
    }

    public void Contamine(float value)
    {
        if (IsContamined() == ContaminationLevel.CONTAMINED)
            return;

        _contaminationLevel += value / contaminationResistance;
        if(IsContamined() == ContaminationLevel.CONTAMINED)
        {
            _contaminationLevel = (int)ContaminationLevel.CONTAMINED;
            StartCoroutine(ContamineCoroutine());
        }
    }

    public ContaminationLevel IsContamined()
    {
        ContaminationLevel level = ContaminationLevel.CONTAMINED;

        foreach (ContaminationLevel value in Enum.GetValues(typeof(ContaminationLevel)))
        {
            if (value > level && (int)value <= _contaminationLevel)
                level = value;
        }

        return level;
    }

    public float ContaminationProportion()
    {
        return _contaminationLevel / ((int)ContaminationLevel.CONTAMINED - (int)ContaminationLevel.UNCONTAMINED);
    }

    IEnumerator ContamineCoroutine()
    {
        while(IsContamined() == ContaminationLevel.CONTAMINED)
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
