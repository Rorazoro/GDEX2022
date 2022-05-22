using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameOverPlayerItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtPlayerName;
        [SerializeField] private TMP_Text txtPlayerTime;
        [SerializeField] private Image txtPlayerPanel;
        [SerializeField] private Image winnerImage;
        [SerializeField] private Color[] playerColors;

        private int playerIndex;

        public void SetPlayerInfo(int index, Photon.Realtime.Player player)
        {
            playerIndex = index;
            txtPlayerName.text = player.NickName;
            txtPlayerPanel.color = playerColors[playerIndex];
            txtPlayerTime.text = FormatTimer((float)player.CustomProperties["PlayerTimer"]);
        }

        public void SetWinner()
        {
            winnerImage.enabled = true;
        }
        
        private string FormatTimer(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int fraction = Mathf.FloorToInt(time * 100 % 100);

            return $"{minutes:00}:{seconds:00}:{fraction:00}";
        }
    }
}