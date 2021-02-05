using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Starter : MonoBehaviour
{
    [SerializeField] bool startServer;
    [SerializeField] GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor && startServer)
        {
            NetworkManager.singleton.StartServer();
        }
        //else if (go != null && !Application.isBatchMode) Destroy(go);
    }
}
