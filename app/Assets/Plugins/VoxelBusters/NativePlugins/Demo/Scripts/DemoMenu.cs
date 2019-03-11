using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins.Demo
{
	public class DemoMenu : MonoBehaviour 
	{
		#region Fields

		[SerializeField]
		private			RectTransform		m_optionsRect			= null;
		[SerializeField]
		private			Button				m_optionButtonPrefab	= null;

		private			MenuOptionData[]	m_options				= new MenuOptionData[]
		{
			new MenuOptionData() { DisplayName = "Address Book", 			LoadSceneName = "AddressBookDemo" },
			new MenuOptionData() { DisplayName = "Alert Dialog", 			LoadSceneName = "AlertDialogDemo" },
			new MenuOptionData() { DisplayName = "Mail Composer", 			LoadSceneName = "MailComposerDemo" },
			new MenuOptionData() { DisplayName = "Message Composer", 		LoadSceneName = "MessageComposerDemo" },
			new MenuOptionData() { DisplayName = "Social Share Composer", 	LoadSceneName = "SocialShareComposerDemo" },
			new MenuOptionData() { DisplayName = "Share Sheet", 			LoadSceneName = "ShareSheetDemo" },
			new MenuOptionData() { DisplayName = "Rate My App", 			LoadSceneName = "RateMyAppDemo" },
		};

		#endregion

		#region Unity methods

		private void Awake()
		{
			// create options
			Transform[] oldChildren = UnityEngineUtility.GetImmediateChildren(m_optionsRect.gameObject);
			for (int iter = 0; iter < oldChildren.Length; iter++)
			{
				Destroy(oldChildren[iter].gameObject);
			}
			for (int iter = 0; iter < m_options.Length; iter++)
			{
				MenuOptionData 	menuOption 	= m_options[iter];
				Button 			newButton 	= Instantiate(m_optionButtonPrefab, m_optionsRect, false);
				newButton.onClick.AddListener(() => SceneManager.LoadScene(menuOption.LoadSceneName));
				newButton.GetComponentInChildren<Text>().text = menuOption.DisplayName;
			}
		}

		#endregion

		#region Nested types

		private struct MenuOptionData
		{
			public string DisplayName { get; set; }
			public string LoadSceneName { get; set; }
		}

		#endregion
	}
}
