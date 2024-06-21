using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private ButtonController buttonPrefab;
        [SerializeField] private Transform buttonParent;

        [SerializeField] private List<ButtonConfig> buttonConfigs = new();
        private List<string> _addedButtonLabels = new List<string>();

        public event Action<string> OnChangeMenu;

        public void Setup()
        {
            foreach (var config in buttonConfigs)
            {
                if (_addedButtonLabels.Contains(config.Label))
                {
                    Debug.LogWarning($"{name}: Button with label {config.Label} has already been added to the menu!\n Ignoring to avoid issues.");
                    continue;
                }

                var newButton = Instantiate(buttonPrefab, buttonParent);
                newButton.name = $"{config.Label}_Btn";
                newButton.Setup(config, HandleButtonClick);
                _addedButtonLabels.Add(config.Label);
            }
        }

        private void HandleButtonClick(string id)
        {
            OnChangeMenu?.Invoke(id);
        }
    }
}