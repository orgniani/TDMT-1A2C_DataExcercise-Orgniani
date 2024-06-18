using System;
using System.Collections.Generic;
using UnityEngine;
using DataSources;
using Gameplay;
using UnityEngine.UIElements;

namespace UI
{
    public class NavigationManager : MonoBehaviour
    {
        [Tooltip("The first item on this list will be set as the default")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private List<string> idsToTellGameManager = new(); //TODO: Change name to a better one - SF
        private int _currentMenuIndex = 0;

        private void Start()
        {
            //TODO: Null check if menus with id list empty - SF

            foreach (var menu in menusWithId)
            {
                //TODO: Add id check to let the user know when 2 menus have the same ID - SF

                menu.Menu.Setup();
                menu.Menu.OnChangeMenu += HandleChangeMenu;
                menu.Menu.gameObject.SetActive(false);
            }

            if (menusWithId.Count > 0)
            {
                menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(true);
            }
        }

        private void HandleChangeMenu(string id)
        {
            if (idsToTellGameManager.Contains(id) && gameManagerDataSource != null && gameManagerDataSource.Value != null)
            {
                gameManagerDataSource.Value.HandleSpecialEvents(id);
            }

            for (var i = 0; i < menusWithId.Count; i++)
            {
                var menuWithId = menusWithId[i];

                if (menuWithId.ID == id)
                {
                    //TODO: Add check to let the user know if _currentMenuIndex is bigger than menusWithId.Count - SF

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
            [field:SerializeField] public string ID { get; set; }
            [field: SerializeField] public Menu Menu { get; set; }
        }
    }
}
