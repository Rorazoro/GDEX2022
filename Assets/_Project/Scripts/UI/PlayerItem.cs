using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class PlayerItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text txtPlayerName;
        [SerializeField] private Image txtPlayerPanel;
        [SerializeField] private Color[] playerColors;

        private int playerIndex;
        private Hashtable playerProperties = new Hashtable();

        public void SetPlayerInfo(int index, Player _player)
        {
            playerIndex = index;
            txtPlayerName.text = _player.NickName;
            txtPlayerPanel.color = playerColors[playerIndex];
            //SetPlayerProperties();
        }

        private void SetPlayerProperties()
        {
            playerProperties["playerIndex"] = playerIndex;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
        
        // public void SetPlayerName(string playerName)
        // {
        //     txtPlayerName.text = playerName;
        //     PlayerName = playerName;
        // }
        //
        // public void SetPlayerColor(Color playerColor)
        // {
        //     txtPlayerName.color = playerColor;
        //     PlayerColor = playerColor;
        // }
        //
        // public void SetPlayerMaster(bool master)
        // {
        //     imgMaster.enabled = master;
        // }
        //
        // public void SetPlayerReady(bool ready)
        // {
        //     imgReady.enabled = ready;
        // }
    }
}