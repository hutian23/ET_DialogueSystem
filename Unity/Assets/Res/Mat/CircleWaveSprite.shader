Shader "Custom/CircleWaveSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _WaveColor ("Wave Color", Color) = (1,1,1,1)
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Base Radius", Range(0, 1)) = 0.5
        _Width ("Wave Width", Range(0, 1)) = 0.1
        _Progress("Wave Progress", Float) = 0.0
        [Toggle(USE_TEXTURE)] _UseTex ("Use Texture", Float) = 0
    }
    
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_TEXTURE
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _WaveColor;
            float2 _Center;
            float _Radius;
            float _Width;
            float _Progress;
            
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                // 计算当前点到中心的距离
                float distanceToCenter = length(IN.texcoord - _Center);
                
                // 计算当前时间
                float waveProgress = frac(_Progress);
                
                // 计算动态半径(波浪位置)
                float dynamicRadius = waveProgress * (1.0 + _Width);
                
                // 计算波浪圆环区域(使用平滑过渡)
                float wave = smoothstep(dynamicRadius - _Width, dynamicRadius, distanceToCenter) * 
                            (1.0 - smoothstep(dynamicRadius, dynamicRadius + _Width, distanceToCenter));
                
                // 基础圆形区域
                float baseCircle = smoothstep(_Radius, _Radius - 0.01, distanceToCenter);
                
                // 默认颜色为纯色
                fixed4 baseColor = IN.color;
                
                // 如果启用纹理，则采样纹理
                #ifdef USE_TEXTURE
                    baseColor *= tex2D(_MainTex, IN.texcoord);
                #endif
                
                // 混合颜色 - 波浪优先于基础圆
                fixed4 col = lerp(baseColor * baseCircle, _WaveColor, wave);

                // 计算总alpha - 显示波浪或基础圆，并应用淡出效果
                col.a = (wave + (baseCircle * (1.0 - wave))) * IN.color.a;
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}