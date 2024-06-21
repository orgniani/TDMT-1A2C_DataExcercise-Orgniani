using System;
using System.Collections.Generic;
using UnityEngine;
using DataSources;
using Gameplay;
using Events;
using Core;
using System.Linq;

namespace UI
{
    public class NavigationManager : MonoBehaviour
    {
        [SerializeField] private string exitButtonId = "Exit";

        [Tooltip("The first item on this list will be set as the default")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private List<ButtonConfig> buttonConfigs = new();

        private int _currentMenuIndex = 0;

        private void Awake()
        {
            //TODO: NULL CHECK FOR EVERY SINGLE CLASS!! -SF
            if (menusWithId == null || menusWithId.Count == 0)
            {
                Debug.LogError($"{name}: MenusWithId list is null or empty!");
            }
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, HandleOpenWinMenu);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoseAction, HandleOpenLoseMenu);
            }
        }

        private void Start()
        {
            var menuIds = new List<string>();

            foreach (var menu in menusWithId)
            {
                if (menuIds.Contains(menu.ID))
                {
                    Debug.LogWarning($"{name}: Menu ID {menu.ID} has already been added! \n Ignoring to avoid issues.");
                    continue;
                }
                menuIds.Add(menu.ID);

                menu.Menu.Setup();
                menu.Menu.OnChangeMenu += HandleMenuOptions;
                menu.Menu.gameObject.SetActive(false);
            }

            menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, HandleOpenWinMenu);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, HandleOpenLoseMenu);
            }
        }

        private void HandleOpenWinMenu(params object[] args)
        {
            if(gameManagerDataSource.Value.IsFinalLevel)
                HandleMenuOptions(GameEvents.WinAction);
        }

        private void HandleOpenLoseMenu(params object[] args)
        {
            if (gameManagerDataSource.Value.IsFinalLevel)
                HandleMenuOptions(GameEvents.LoseAction);
        }


        private void HandleMenuOptions(string id)
        {
            if (id == exitButtonId) //TODO: Make this look better -SF
            {
                ExitGame();
                return;
            }

            if (buttonConfigs.Any(config => config.Label == id) && gameManagerDataSource.Value != null)
            {
                gameManagerDataSource.Value.HandlePlayGame();
                menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(false);
            }

            for (var i = 0; i < menusWithId.Count; i++)
            {
                var menuWithId = menusWithId[i];

                if (menuWithId.ID == id)
                {
                    if (_currentMenuIndex >= menusWithId.Count)
                    {
                        Debug.Log($"{name}: CurrentMenuIndex {_currentMenuIndex} is out of bounds.");
                        return;
                    }

                    menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(false);
                    menuWithId.Menu.gameObject.SetActive(true);
                    _currentMenuIndex = i;
                    break;
                }
            }
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        [Serializable]
        public struct MenuWithId
        {
            [field : SerializeField] public string ID { get; set; }
            [field : SerializeField] public Menu Menu { get; set; }
        }
    }
}
