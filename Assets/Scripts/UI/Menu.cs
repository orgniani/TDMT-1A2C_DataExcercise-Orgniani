using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private ButtonController buttonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private List<string> ids = new();

        public event Action<string> OnChangeMenu;

        public void Setup()
        {
            foreach (var id in ids)
            {
                var newButton = Instantiate(buttonPrefab, buttonParent);
                newButton.name = $"{id}_Btn";
                newButton.Setup(id, id, HandleButtonClick);
            }
        }

        private void HandleButtonClick(string id)
        {
            OnChangeMenu?.Invoke(id);
        }
    }
}