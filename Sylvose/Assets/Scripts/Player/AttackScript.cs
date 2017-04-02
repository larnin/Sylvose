using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float radius = 2.0f;
    public float time = 0.3f;

    //Ne pas detruire les orbres
    public GameObject orb;

    float _life = 0.0f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var o in objects)
        {
            var tag = o.gameObject.tag; //
            var contamineCompent = o.gameObject.GetComponent<Contaminable>();
            if (contamineCompent == null)
                continue;
            if (contamineCompent.IsContamined() == Contaminable.ContaminationLevel.CONTAMINED && tag != "Orb")
                Destroy(o.gameObject);

        }

        _life += Time.deltaTime;
        if (_life > time)
            Destroy(gameObject);
	}
}
