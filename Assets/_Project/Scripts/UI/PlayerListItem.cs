using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class PlayerListItem : MonoBehaviour
    {
        [SerializeField] TMP_Text txtPlayerName;
        
        public string PlayerName;
        public Color PlayerColor;

        public void SetPlayerName(string playerName)
        {
            txtPlayerName.text = playerName;
            PlayerName = playerName;
        }

        public void SetPlayerColor(Color playerColor)
        {
            txtPlayerName.color = playerColor;
            PlayerColor = playerColor;
        }
    }
}