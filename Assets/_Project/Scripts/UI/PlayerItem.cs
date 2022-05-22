using Photon.Pun;
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

        public void SetPlayerInfo(int index, Photon.Realtime.Player player)
        {
            playerIndex = index;
            txtPlayerName.text = player.NickName;
            txtPlayerPanel.color = playerColors[playerIndex];
        }
    }
}