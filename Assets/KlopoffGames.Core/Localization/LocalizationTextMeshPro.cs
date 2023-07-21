using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace KlopoffGames.Core.Localization
{
	public class LocalizationTextMeshPro : MonoBehaviour 
	{
		[SerializeField] private string id;

		private LocalizationManager _localizationManager;
		private TextMeshPro _text;

		[Inject]
		public void Construct(LocalizationManager localizationManager)
		{
			_localizationManager = localizationManager;
		}

		private void OnValidate()
		{
			if (GetComponent<TextMeshPro>() == null)
			{
				throw new NullReferenceException($"No {nameof(TextMeshPro)} attached to gameObject");
			}
		}

		private void OnEnable()
		{
			_text = GetComponent<TextMeshPro>();
			
			if (_localizationManager != null)
			{
				_localizationManager.OnLocalizationLoad += OnLocalizationLoad;
				UpdateText();
			}
		}

		private void OnDisable()
		{
			if (_localizationManager != null)
			{
				_localizationManager.OnLocalizationLoad -= OnLocalizationLoad;
			}
		}

		public void UpdateText () 
		{
			if (_text != null)
			{
				_text.text = _localizationManager.GetString(id);
			}
		}
	
		private void OnLocalizationLoad(string language)
		{
			UpdateText();
		}
	}
}
