using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Test.Saper
{
    public class Server : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void OnStartServer()
        {
            Debug.Log("Server is up!");
        }

        public override void OnStopServer()
        {
            Debug.Log("Server is down!");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
