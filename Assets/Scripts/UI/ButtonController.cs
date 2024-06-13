using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public sealed class ButtonController : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;

        private string _id;
        private Button _button;

        public event Action<string> OnClick;

        private void Reset()
        {
            GameObject child;

            if (transform.childCount < 1)
            {
                child = new GameObject("Text (TMP)");
                child.transform.SetParent(transform);
            }

            else child = transform.GetChild(0).gameObject;

            if (!child.TryGetComponent<TMP_Text>(out buttonText))
            {
                buttonText = child.AddComponent<TextMeshProUGUI>();
            }

            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            buttonText ??= GetComponent<TMP_Text>();
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public void Setup(string label, string id, Action<string> onClick)
        {
            buttonText.SetText(label);
            _id = id;
            OnClick = onClick;
        }

        private void HandleButtonClick()
        {
            OnClick?.Invoke(_id);
        }
    }
}
