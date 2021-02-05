using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Test.Saper
{
    public class NetMgr : NetworkManager
    {
        [Header("Custom")]
        [SerializeField] GameObject NetEntity;
        PlayerList PlayerList;

        static readonly ILogger logger = LogFactory.GetLogger<NetMgr>();

        public override void Start()
        {
            //DontDestroyOnLoad(NetEntity); //DO NOT DO THIS
            PlayerList = NetEntity.GetComponent<PlayerList>();
            base.Start();
        }

        public override void OnValidate()
        {
            base.OnValidate();
            Debug.Assert(playerPrefab != null, "NetMgr - playerPrefab cannot be null!");
        }

        #region Server
        public override void OnStartServer()
        {
            NetworkServer.Spawn(NetEntity);
            Debug.Log("Server started!");
        }

        public override void OnStopServer()
        {
            Debug.Log("Server stopped!");
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            Debug.Log($"Player :{conn.address}:{{{conn.connectionId}}} connected");
            if (!PlayerList.AddConnectedClient(conn, new Player()))
            {
                Debug.LogError($"Unexpected error {conn.address}. Disconnected"); //TODO: send message
                conn.Disconnect();
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            Player player = PlayerList.GetPlayer(conn.connectionId);
            PlayerList.MoveClientToActive(conn);
            if (player != null)
            {
                if (autoCreatePlayer)
                {
                    GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, new Quaternion());
                    NetworkServer.AddPlayerForConnection(conn, playerObj);
                }
                
                Debug.Log($"Player {{{player.name}}} joined the game");
            }
            else
            {
                Debug.LogError($"Player not found: {conn.address}. Disconnected");
                conn.Disconnect();
            }
        }

        //TODO: is it called if client did not authenticate?
        //TODO: end function
        //TODO: add disconnect message (with message just before this msg)
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            PlayerList.RemoveClient(conn);
            Debug.Log($"Client {conn.address} {{{conn.connectionId}}} disconnected");
        }
        #endregion

        #region Client
        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("Connected to the server");
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            Debug.Log("Disconnected from the server");
        }
        #endregion
    }
}
