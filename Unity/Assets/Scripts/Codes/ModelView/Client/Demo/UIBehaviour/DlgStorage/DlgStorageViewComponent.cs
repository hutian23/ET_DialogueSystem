
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgStorage))]
	[EnableMethod]
	public  class DlgStorageViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.LoopVerticalScrollRect E_StorageLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_StorageLoopVerticalScrollRect == null )
     			{
		    		this.m_E_StorageLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_Storage");
     			}
     			return this.m_E_StorageLoopVerticalScrollRect;
     		}
     	}

		public UnityEngine.UI.Image E_ConfirmImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ConfirmImage == null )
     			{
		    		this.m_E_ConfirmImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Confirm");
     			}
     			return this.m_E_ConfirmImage;
     		}
     	}

		public UnityEngine.UI.Button E_ConfirmBtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ConfirmBtnButton == null )
     			{
		    		this.m_E_ConfirmBtnButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Confirm/E_ConfirmBtn");
     			}
     			return this.m_E_ConfirmBtnButton;
     		}
     	}

		public UnityEngine.UI.Image E_ConfirmBtnImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ConfirmBtnImage == null )
     			{
		    		this.m_E_ConfirmBtnImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Confirm/E_ConfirmBtn");
     			}
     			return this.m_E_ConfirmBtnImage;
     		}
     	}

		public UnityEngine.UI.Button E_CancelButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CancelButton == null )
     			{
		    		this.m_E_CancelButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Confirm/E_Cancel");
     			}
     			return this.m_E_CancelButton;
     		}
     	}

		public UnityEngine.UI.Image E_CancelImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CancelImage == null )
     			{
		    		this.m_E_CancelImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Confirm/E_Cancel");
     			}
     			return this.m_E_CancelImage;
     		}
     	}

		public UnityEngine.UI.Button E_SaveButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SaveButton == null )
     			{
		    		this.m_E_SaveButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Save");
     			}
     			return this.m_E_SaveButton;
     		}
     	}

		public UnityEngine.UI.Image E_SaveImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SaveImage == null )
     			{
		    		this.m_E_SaveImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Save");
     			}
     			return this.m_E_SaveImage;
     		}
     	}

		public UnityEngine.UI.Button E_LoadButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoadButton == null )
     			{
		    		this.m_E_LoadButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Load");
     			}
     			return this.m_E_LoadButton;
     		}
     	}

		public UnityEngine.UI.Image E_LoadImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoadImage == null )
     			{
		    		this.m_E_LoadImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Load");
     			}
     			return this.m_E_LoadImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_StorageLoopVerticalScrollRect = null;
			this.m_E_ConfirmImage = null;
			this.m_E_ConfirmBtnButton = null;
			this.m_E_ConfirmBtnImage = null;
			this.m_E_CancelButton = null;
			this.m_E_CancelImage = null;
			this.m_E_SaveButton = null;
			this.m_E_SaveImage = null;
			this.m_E_LoadButton = null;
			this.m_E_LoadImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.LoopVerticalScrollRect m_E_StorageLoopVerticalScrollRect = null;
		private UnityEngine.UI.Image m_E_ConfirmImage = null;
		private UnityEngine.UI.Button m_E_ConfirmBtnButton = null;
		private UnityEngine.UI.Image m_E_ConfirmBtnImage = null;
		private UnityEngine.UI.Button m_E_CancelButton = null;
		private UnityEngine.UI.Image m_E_CancelImage = null;
		private UnityEngine.UI.Button m_E_SaveButton = null;
		private UnityEngine.UI.Image m_E_SaveImage = null;
		private UnityEngine.UI.Button m_E_LoadButton = null;
		private UnityEngine.UI.Image m_E_LoadImage = null;
		public Transform uiTransform = null;
	}
}
