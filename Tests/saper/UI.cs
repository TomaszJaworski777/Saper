using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Test.Saper
{
    public class UI : MonoBehaviour
    {
        [SerializeField] Button conn;
        [SerializeField] Button rdy;
        [SerializeField] Button dc;

        void Start()
        {
            conn.onClick.AddListener(Connect);
            rdy.onClick.AddListener(Ready);
            dc.onClick.AddListener(Disconnect);
        }

        void Connect()
        {
            NetworkManager.singleton.StartClient();
        }

        void Ready()
        {
            ClientScene.Ready(NetworkClient.connection);
        }

        void Disconnect()
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
