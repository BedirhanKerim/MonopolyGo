using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonopolyGo
{
    public class DiceInputUI : MonoBehaviour
    {
        private const int k_MinValue = 1;
        private const int k_MaxValue = 6;

        [SerializeField] private TMP_InputField m_FirstDiceField;
        [SerializeField] private TMP_InputField m_SecondDiceField;
        [SerializeField] private Button m_RollButton;

        private DiceRoller m_Roller;

        public void Init(DiceRoller roller)
        {
            m_Roller = roller;
        }

        private void OnEnable()
        {
            m_FirstDiceField.onValueChanged.AddListener(OnFirstDiceChanged);
            m_SecondDiceField.onValueChanged.AddListener(OnSecondDiceChanged);
            m_RollButton.onClick.AddListener(OnRollClicked);
            GameEvents.DiceRollCompleted += OnRollCompleted;
        }

        private void OnDisable()
        {
            m_FirstDiceField.onValueChanged.RemoveListener(OnFirstDiceChanged);
            m_SecondDiceField.onValueChanged.RemoveListener(OnSecondDiceChanged);
            m_RollButton.onClick.RemoveListener(OnRollClicked);
            GameEvents.DiceRollCompleted -= OnRollCompleted;
        }

        private void OnRollClicked()
        {
            if (m_Roller == null || m_Roller.IsRolling)
            {
                return;
            }

            var values = new List<int>
            {
                ReadField(m_FirstDiceField),
                ReadField(m_SecondDiceField)
            };

            m_RollButton.interactable = false;
            m_Roller.Roll(values);
        }

        private void OnRollCompleted(int sum)
        {
            m_RollButton.interactable = true;
        }

        private void OnFirstDiceChanged(string text)
        {
            ClampField(m_FirstDiceField, text);
        }

        private void OnSecondDiceChanged(string text)
        {
            ClampField(m_SecondDiceField, text);
        }

        private void ClampField(TMP_InputField field, string text)
        {
            // Empty is allowed while typing; clamp only once there is a number.
            if (string.IsNullOrEmpty(text) || !int.TryParse(text, out int value))
            {
                return;
            }

            int clamped = Mathf.Clamp(value, k_MinValue, k_MaxValue);
            if (clamped != value)
            {
                field.SetTextWithoutNotify(clamped.ToString());
            }
        }

        private int ReadField(TMP_InputField field)
        {
            if (int.TryParse(field.text, out int value))
            {
                return Mathf.Clamp(value, k_MinValue, k_MaxValue);
            }

            return k_MinValue;
        }
    }
}
