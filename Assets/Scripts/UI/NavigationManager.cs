using System;
using System.Collections.Generic;
using UnityEngine;
using DataSources;
using Gameplay;
using Events;
using Core;

namespace UI
{
    public class NavigationManager : MonoBehaviour
    {
        [Tooltip("The first item on this list will be set as the default")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private List<string> idsToTellGameManager = new(); //TODO: Change name to a better one - SF
        
        
        private int _currentMenuIndex = 0;

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.Win, HandleOpenWinMenu);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.Lose, HandleOpenLoseMenu);
            }
        }

        private void Start()
        {
            //TODO: Null check if menus with id list empty - SF
            if (menusWithId == null || menusWithId.Count == 0) return;

            foreach (var menu in menusWithId)
            {
                //TODO: Add id check to let the user know when 2 menus have the same ID - SF

                menu.Menu.Setup();
                menu.Menu.OnChangeMenu += HandleChangeMenu;
                menu.Menu.gameObject.SetActive(false);
            }

            menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.Win, HandleOpenWinMenu);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.Lose, HandleOpenLoseMenu);
            }
        }

        private void HandleOpenWinMenu(params object[] args)
        {
            if(gameManagerDataSource.Value.IsFinalLevel)
                HandleChangeMenu(GameEvents.Win);
        }

        private void HandleOpenLoseMenu(params object[] args)
        {
            if (gameManagerDataSource.Value.IsFinalLevel)
                HandleChangeMenu(GameEvents.Lose);
        }


        private void HandleChangeMenu(string id)
        {
            if (idsToTellGameManager.Contains(id) && gameManagerDataSource != null && gameManagerDataSource.Value != null)
            {
                gameManagerDataSource.Value.HandleSpecialEvents(id);
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

        [Serializable]
        public struct MenuWithId
        {
            [field : SerializeField] public string ID { get; set; }
            [field : SerializeField] public Menu Menu { get; set; }
        }
    }
}
