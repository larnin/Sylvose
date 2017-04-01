using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaminablePlant: Contaminable
{
    BoxCollider2D _box;

    protected override void Start()
    {
        base.Start();
        _box = GetComponent<BoxCollider2D>();
    }

    protected override IEnumerator ContamineCoroutine()
    {
        while(IsContamined() == ContaminationLevel.CONTAMINED)
        {
            float value = spreadPower * Time.deltaTime;
            var box = _box.bounds;
            var objects = Physics2D.OverlapBoxAll(box.center, box.size + new Vector3(contaminationRadius, contaminationRadius, 0), 0);
            foreach(var o in objects)
            {
                var contamineCompent = o.gameObject.GetComponent<Contaminable>();
                if (contamineCompent == null)
                    continue;
                Debug.Log("contamine");
                contamineCompent.Contamine(value);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
