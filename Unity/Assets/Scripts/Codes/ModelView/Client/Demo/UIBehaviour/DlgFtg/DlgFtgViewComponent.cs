
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgFtg))]
	[EnableMethod]
	public  class DlgFtgViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Image E_Arrow_DownImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_DownImage == null )
     			{
		    		this.m_E_Arrow_DownImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_Down");
     			}
     			return this.m_E_Arrow_DownImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_DownRightImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_DownRightImage == null )
     			{
		    		this.m_E_Arrow_DownRightImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_DownRight");
     			}
     			return this.m_E_Arrow_DownRightImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_RightImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_RightImage == null )
     			{
		    		this.m_E_Arrow_RightImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_Right");
     			}
     			return this.m_E_Arrow_RightImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_UpRightImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_UpRightImage == null )
     			{
		    		this.m_E_Arrow_UpRightImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_UpRight");
     			}
     			return this.m_E_Arrow_UpRightImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_UpImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_UpImage == null )
     			{
		    		this.m_E_Arrow_UpImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_Up");
     			}
     			return this.m_E_Arrow_UpImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_UpLeftImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_UpLeftImage == null )
     			{
		    		this.m_E_Arrow_UpLeftImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_UpLeft");
     			}
     			return this.m_E_Arrow_UpLeftImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_LeftImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_LeftImage == null )
     			{
		    		this.m_E_Arrow_LeftImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_Left");
     			}
     			return this.m_E_Arrow_LeftImage;
     		}
     	}

		public UnityEngine.UI.Image E_Arrow_DownLeftImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_Arrow_DownLeftImage == null )
     			{
		    		this.m_E_Arrow_DownLeftImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_Arrow_DownLeft");
     			}
     			return this.m_E_Arrow_DownLeftImage;
     		}
     	}

		public UnityEngine.UI.Image E_LightPunchImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LightPunchImage == null )
     			{
		    		this.m_E_LightPunchImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_LightPunch");
     			}
     			return this.m_E_LightPunchImage;
     		}
     	}

		public UnityEngine.UI.Image E_LightKickImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LightKickImage == null )
     			{
		    		this.m_E_LightKickImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_LightKick");
     			}
     			return this.m_E_LightKickImage;
     		}
     	}

		public UnityEngine.UI.Image E_MiddleKickImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MiddleKickImage == null )
     			{
		    		this.m_E_MiddleKickImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_MiddleKick");
     			}
     			return this.m_E_MiddleKickImage;
     		}
     	}

		public UnityEngine.UI.Image E_MiddlePunchImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MiddlePunchImage == null )
     			{
		    		this.m_E_MiddlePunchImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_MiddlePunch");
     			}
     			return this.m_E_MiddlePunchImage;
     		}
     	}

		public UnityEngine.UI.Image E_HeavyPunchImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_HeavyPunchImage == null )
     			{
		    		this.m_E_HeavyPunchImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_HeavyPunch");
     			}
     			return this.m_E_HeavyPunchImage;
     		}
     	}

		public UnityEngine.UI.Image E_HeavyKickImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_HeavyKickImage == null )
     			{
		    		this.m_E_HeavyKickImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/GamePad/E_HeavyKick");
     			}
     			return this.m_E_HeavyKickImage;
     		}
     	}

		public UnityEngine.UI.LoopVerticalScrollRect ELoopScrollList_InputLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELoopScrollList_InputLoopVerticalScrollRect == null )
     			{
		    		this.m_ELoopScrollList_InputLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"background/ELoopScrollList_Input");
     			}
     			return this.m_ELoopScrollList_InputLoopVerticalScrollRect;
     		}
     	}

		public UnityEngine.UI.Image E_P1Image
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_P1Image == null )
     			{
		    		this.m_E_P1Image = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/E_P1");
     			}
     			return this.m_E_P1Image;
     		}
     	}

		public UnityEngine.UI.Text E_P1_FrameInfoText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_P1_FrameInfoText == null )
     			{
		    		this.m_E_P1_FrameInfoText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"background/E_P1/E_P1_FrameInfo");
     			}
     			return this.m_E_P1_FrameInfoText;
     		}
     	}

		public UnityEngine.UI.Image E_P2Image
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_P2Image == null )
     			{
		    		this.m_E_P2Image = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"background/E_P2");
     			}
     			return this.m_E_P2Image;
     		}
     	}

		public UnityEngine.UI.Text E_P2_FrameInfoText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_P2_FrameInfoText == null )
     			{
		    		this.m_E_P2_FrameInfoText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"background/E_P2/E_P2_FrameInfo");
     			}
     			return this.m_E_P2_FrameInfoText;
     		}
     	}

		public UnityEngine.UI.Text E_SkillText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SkillText == null )
     			{
		    		this.m_E_SkillText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_Skill");
     			}
     			return this.m_E_SkillText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_Arrow_DownImage = null;
			this.m_E_Arrow_DownRightImage = null;
			this.m_E_Arrow_RightImage = null;
			this.m_E_Arrow_UpRightImage = null;
			this.m_E_Arrow_UpImage = null;
			this.m_E_Arrow_UpLeftImage = null;
			this.m_E_Arrow_LeftImage = null;
			this.m_E_Arrow_DownLeftImage = null;
			this.m_E_LightPunchImage = null;
			this.m_E_LightKickImage = null;
			this.m_E_MiddleKickImage = null;
			this.m_E_MiddlePunchImage = null;
			this.m_E_HeavyPunchImage = null;
			this.m_E_HeavyKickImage = null;
			this.m_ELoopScrollList_InputLoopVerticalScrollRect = null;
			this.m_E_P1Image = null;
			this.m_E_P1_FrameInfoText = null;
			this.m_E_P2Image = null;
			this.m_E_P2_FrameInfoText = null;
			this.m_E_SkillText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Image m_E_Arrow_DownImage = null;
		private UnityEngine.UI.Image m_E_Arrow_DownRightImage = null;
		private UnityEngine.UI.Image m_E_Arrow_RightImage = null;
		private UnityEngine.UI.Image m_E_Arrow_UpRightImage = null;
		private UnityEngine.UI.Image m_E_Arrow_UpImage = null;
		private UnityEngine.UI.Image m_E_Arrow_UpLeftImage = null;
		private UnityEngine.UI.Image m_E_Arrow_LeftImage = null;
		private UnityEngine.UI.Image m_E_Arrow_DownLeftImage = null;
		private UnityEngine.UI.Image m_E_LightPunchImage = null;
		private UnityEngine.UI.Image m_E_LightKickImage = null;
		private UnityEngine.UI.Image m_E_MiddleKickImage = null;
		private UnityEngine.UI.Image m_E_MiddlePunchImage = null;
		private UnityEngine.UI.Image m_E_HeavyPunchImage = null;
		private UnityEngine.UI.Image m_E_HeavyKickImage = null;
		private UnityEngine.UI.LoopVerticalScrollRect m_ELoopScrollList_InputLoopVerticalScrollRect = null;
		private UnityEngine.UI.Image m_E_P1Image = null;
		private UnityEngine.UI.Text m_E_P1_FrameInfoText = null;
		private UnityEngine.UI.Image m_E_P2Image = null;
		private UnityEngine.UI.Text m_E_P2_FrameInfoText = null;
		private UnityEngine.UI.Text m_E_SkillText = null;
		public Transform uiTransform = null;
	}
}
