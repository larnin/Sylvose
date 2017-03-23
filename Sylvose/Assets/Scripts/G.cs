using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class G
{
    private static volatile G _instance;
    private PlayerComportement _playerComportement;

    public static G Sys
    {
        get
        {
            if (G._instance == null)
                G._instance = new G();
            return G._instance;
        }
    }

    public PlayerComportement playerComportement
    {
        set
        {
            if (_playerComportement != null)
                Debug.Log("2 PlayerComportement instanciated !");
            _playerComportement = value;
        }
        get { return _playerComportement; }
    }

}
