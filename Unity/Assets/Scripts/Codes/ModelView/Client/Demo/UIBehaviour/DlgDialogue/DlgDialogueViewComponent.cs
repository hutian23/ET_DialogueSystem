
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
		    		this.m_E_TextText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Background/E_Text");
     			}
     			return this.m_E_TextText;
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
			this.m_E_TextText = null;
			this.m_E_QuickSaveButton = null;
			this.m_E_QuickSaveImage = null;
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
		private UnityEngine.UI.Text m_E_TextText = null;
		private UnityEngine.UI.Button m_E_QuickSaveButton = null;
		private UnityEngine.UI.Image m_E_QuickSaveImage = null;
		public Transform uiTransform = null;
	}
}
