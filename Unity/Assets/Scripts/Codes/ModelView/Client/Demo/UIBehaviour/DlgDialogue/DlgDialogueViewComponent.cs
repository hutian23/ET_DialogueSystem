
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgDialogue))]
	[EnableMethod]
	public  class DlgDialogueViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button E_ClearQSButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ClearQSButton == null )
     			{
		    		this.m_E_ClearQSButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_ClearQS");
     			}
     			return this.m_E_ClearQSButton;
     		}
     	}

		public UnityEngine.UI.Image E_ClearQSImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ClearQSImage == null )
     			{
		    		this.m_E_ClearQSImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_ClearQS");
     			}
     			return this.m_E_ClearQSImage;
     		}
     	}

		public UnityEngine.UI.Button E_CheckQuickSaveButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckQuickSaveButton == null )
     			{
		    		this.m_E_CheckQuickSaveButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_CheckQuickSave");
     			}
     			return this.m_E_CheckQuickSaveButton;
     		}
     	}

		public UnityEngine.UI.Image E_CheckQuickSaveImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckQuickSaveImage == null )
     			{
		    		this.m_E_CheckQuickSaveImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_CheckQuickSave");
     			}
     			return this.m_E_CheckQuickSaveImage;
     		}
     	}

		public UnityEngine.UI.InputField E_CheckInput_TreeIDInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckInput_TreeIDInputField == null )
     			{
		    		this.m_E_CheckInput_TreeIDInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"E_CheckInput_TreeID");
     			}
     			return this.m_E_CheckInput_TreeIDInputField;
     		}
     	}

		public UnityEngine.UI.Image E_CheckInput_TreeIDImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckInput_TreeIDImage == null )
     			{
		    		this.m_E_CheckInput_TreeIDImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_CheckInput_TreeID");
     			}
     			return this.m_E_CheckInput_TreeIDImage;
     		}
     	}

		public UnityEngine.UI.InputField E_CheckInput_TargetIDInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckInput_TargetIDInputField == null )
     			{
		    		this.m_E_CheckInput_TargetIDInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"E_CheckInput_TargetID");
     			}
     			return this.m_E_CheckInput_TargetIDInputField;
     		}
     	}

		public UnityEngine.UI.Image E_CheckInput_TargetIDImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CheckInput_TargetIDImage == null )
     			{
		    		this.m_E_CheckInput_TargetIDImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_CheckInput_TargetID");
     			}
     			return this.m_E_CheckInput_TargetIDImage;
     		}
     	}

		public UnityEngine.UI.Image E_ContentImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ContentImage == null )
     			{
		    		this.m_E_ContentImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Content");
     			}
     			return this.m_E_ContentImage;
     		}
     	}

		public UnityEngine.UI.Text E_TextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TextText == null )
     			{
		    		this.m_E_TextText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_Content/E_Text");
     			}
     			return this.m_E_TextText;
     		}
     	}

		public UnityEngine.UI.Button E_LeftArrowButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LeftArrowButton == null )
     			{
		    		this.m_E_LeftArrowButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Content/E_LeftArrow");
     			}
     			return this.m_E_LeftArrowButton;
     		}
     	}

		public UnityEngine.UI.Image E_LeftArrowImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LeftArrowImage == null )
     			{
		    		this.m_E_LeftArrowImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Content/E_LeftArrow");
     			}
     			return this.m_E_LeftArrowImage;
     		}
     	}

		public UnityEngine.UI.Button E_RightArrowButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RightArrowButton == null )
     			{
		    		this.m_E_RightArrowButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Content/E_RightArrow");
     			}
     			return this.m_E_RightArrowButton;
     		}
     	}

		public UnityEngine.UI.Image E_RightArrowImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RightArrowImage == null )
     			{
		    		this.m_E_RightArrowImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Content/E_RightArrow");
     			}
     			return this.m_E_RightArrowImage;
     		}
     	}

		public UnityEngine.UI.Text E_characterNameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_characterNameText == null )
     			{
		    		this.m_E_characterNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_Content/Character/E_characterName");
     			}
     			return this.m_E_characterNameText;
     		}
     	}

		public UnityEngine.UI.Button E_QuickSaveButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_QuickSaveButton == null )
     			{
		    		this.m_E_QuickSaveButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_QuickSave");
     			}
     			return this.m_E_QuickSaveButton;
     		}
     	}

		public UnityEngine.UI.Image E_QuickSaveImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_QuickSaveImage == null )
     			{
		    		this.m_E_QuickSaveImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_QuickSave");
     			}
     			return this.m_E_QuickSaveImage;
     		}
     	}

		public UnityEngine.UI.LoopVerticalScrollRect E_ChoicePanelLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ChoicePanelLoopVerticalScrollRect == null )
     			{
		    		this.m_E_ChoicePanelLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_ChoicePanel");
     			}
     			return this.m_E_ChoicePanelLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_ClearQSButton = null;
			this.m_E_ClearQSImage = null;
			this.m_E_CheckQuickSaveButton = null;
			this.m_E_CheckQuickSaveImage = null;
			this.m_E_CheckInput_TreeIDInputField = null;
			this.m_E_CheckInput_TreeIDImage = null;
			this.m_E_CheckInput_TargetIDInputField = null;
			this.m_E_CheckInput_TargetIDImage = null;
			this.m_E_ContentImage = null;
			this.m_E_TextText = null;
			this.m_E_LeftArrowButton = null;
			this.m_E_LeftArrowImage = null;
			this.m_E_RightArrowButton = null;
			this.m_E_RightArrowImage = null;
			this.m_E_characterNameText = null;
			this.m_E_QuickSaveButton = null;
			this.m_E_QuickSaveImage = null;
			this.m_E_ChoicePanelLoopVerticalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_E_ClearQSButton = null;
		private UnityEngine.UI.Image m_E_ClearQSImage = null;
		private UnityEngine.UI.Button m_E_CheckQuickSaveButton = null;
		private UnityEngine.UI.Image m_E_CheckQuickSaveImage = null;
		private UnityEngine.UI.InputField m_E_CheckInput_TreeIDInputField = null;
		private UnityEngine.UI.Image m_E_CheckInput_TreeIDImage = null;
		private UnityEngine.UI.InputField m_E_CheckInput_TargetIDInputField = null;
		private UnityEngine.UI.Image m_E_CheckInput_TargetIDImage = null;
		private UnityEngine.UI.Image m_E_ContentImage = null;
		private UnityEngine.UI.Text m_E_TextText = null;
		private UnityEngine.UI.Button m_E_LeftArrowButton = null;
		private UnityEngine.UI.Image m_E_LeftArrowImage = null;
		private UnityEngine.UI.Button m_E_RightArrowButton = null;
		private UnityEngine.UI.Image m_E_RightArrowImage = null;
		private UnityEngine.UI.Text m_E_characterNameText = null;
		private UnityEngine.UI.Button m_E_QuickSaveButton = null;
		private UnityEngine.UI.Image m_E_QuickSaveImage = null;
		private UnityEngine.UI.LoopVerticalScrollRect m_E_ChoicePanelLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
