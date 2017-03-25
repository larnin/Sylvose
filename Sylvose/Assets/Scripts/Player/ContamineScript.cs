using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContamineScript : MonoBehaviour
{
    public float radius = 2.0f;
    public float time = 0.2f;
    public float power = 5.0f;

    float _life = 0.0f;
    List<GameObject> _contaminedObjects = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var o in objects)
        {
            if (_contaminedObjects.Exists(obj => { return obj == o.gameObject; }))
                continue;
            var contamineCompent = o.gameObject.GetComponent<Contaminable>();
            if (contamineCompent == null)
                continue;
            contamineCompent.Contamine(power);
            _contaminedObjects.Add(o.gameObject);
        }

        _life += Time.deltaTime;
        if (_life > time)
            Destroy(gameObject);
    }
}
