
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgTest))]
	[EnableMethod]
	public  class DlgTestViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button E_LoginButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginButton == null )
     			{
		    		this.m_E_LoginButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Sprite_BackGround/E_Login");
     			}
     			return this.m_E_LoginButton;
     		}
     	}

		public UnityEngine.UI.Image E_LoginImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginImage == null )
     			{
		    		this.m_E_LoginImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Sprite_BackGround/E_Login");
     			}
     			return this.m_E_LoginImage;
     		}
     	}

		public UnityEngine.UI.InputField E_SceneInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SceneInputField == null )
     			{
		    		this.m_E_SceneInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"Sprite_BackGround/E_Scene");
     			}
     			return this.m_E_SceneInputField;
     		}
     	}

		public UnityEngine.UI.Image E_SceneImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SceneImage == null )
     			{
		    		this.m_E_SceneImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Sprite_BackGround/E_Scene");
     			}
     			return this.m_E_SceneImage;
     		}
     	}

		public UnityEngine.UI.LoopHorizontalScrollRect ELoopScrollList_testLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELoopScrollList_testLoopHorizontalScrollRect == null )
     			{
		    		this.m_ELoopScrollList_testLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"Sprite_BackGround/ELoopScrollList_test");
     			}
     			return this.m_ELoopScrollList_testLoopHorizontalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_LoginButton = null;
			this.m_E_LoginImage = null;
			this.m_E_SceneInputField = null;
			this.m_E_SceneImage = null;
			this.m_ELoopScrollList_testLoopHorizontalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_E_LoginButton = null;
		private UnityEngine.UI.Image m_E_LoginImage = null;
		private UnityEngine.UI.InputField m_E_SceneInputField = null;
		private UnityEngine.UI.Image m_E_SceneImage = null;
		private UnityEngine.UI.LoopHorizontalScrollRect m_ELoopScrollList_testLoopHorizontalScrollRect = null;
		public Transform uiTransform = null;
	}
}
