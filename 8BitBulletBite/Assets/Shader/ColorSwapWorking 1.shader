Shader "Hidden/ColorSwap_Shadow"
{
	Properties
	{
		[PerRendererData]_MainTex("Texture", 2D) = "white" {}
		_MaskTex("Shadowing", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0



			_Color1in("Color 1 In", Color) = (1,1,1,1)
			_Color2in("Color 2 In", Color) = (1,1,1,1)
			_Color3in("Color 3 In", Color) = (1,1,1,1)
			_Color4in("Color 4 In", Color) = (1,1,1,1)
			_Color5in("Color 5 In", Color) = (1,1,1,1)
			_Color6in("Color 6 In", Color) = (1,1,1,1)
			_Color7in("Color 7 In", Color) = (1,1,1,1)
			_Color8in("Color 8 In", Color) = (1,1,1,1)

			_ColorTresh("Color Treshold", Range(0, 1)) = 0.1

			_Color1out("Color 1 Out", Color) = (1,1,1,1)
			_Color2out("Color 2 Out", Color) = (1,1,1,1)
			_Color3out("Color 3 Out", Color) = (1,1,1,1)
			_Color4out("Color 4 Out", Color) = (1,1,1,1)
			_Color5out("Color 5 Out", Color) = (1,1,1,1)
			_Color6out("Color 6 Out", Color) = (1,1,1,1)
			_Color7out("Color 7 Out", Color) = (1,1,1,1)
			_Color8out("Color 8 Out", Color) = (1,1,1,1)
	}
		SubShader
		{


				Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
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
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv1 : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float4 color    : COLOR;
				};

				struct v2f
				{
					float2 uv1 : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
				};

				fixed4 _Color1in;
				fixed4 _Color2in;
				fixed4 _Color3in;
				fixed4 _Color4in;
				fixed4 _Color5in;
				fixed4 _Color6in;
				fixed4 _Color7in;
				fixed4 _Color8in;

				fixed4 _Color1out;
				fixed4 _Color2out;
				fixed4 _Color3out;
				fixed4 _Color4out;
				fixed4 _Color5out;
				fixed4 _Color6out;
				fixed4 _Color7out;
				fixed4 _Color8out;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv1 = v.uv1;
					o.uv2 = v.uv2;
					o.color = v.color;

					#ifdef PIXELSNAP_ON
					o.vertex = UnityPixelSnap(o.vertex);
					#endif

					return o;
				}

				sampler2D _MainTex;
				sampler2D _MaskTex;
				float _ColorTresh;


				fixed4 frag(v2f i) : COLOR
				{
					half4 col = tex2D(_MainTex, i.uv1.xy);

					col = all(abs(col.r - _Color1in.r) <= _ColorTresh && abs(col.g - _Color1in.g) <= _ColorTresh && abs(col.b - _Color1in.b) <= _ColorTresh) ? _Color1out : col;
					col = all(abs(col.r - _Color2in.r) <= _ColorTresh && abs(col.g - _Color2in.g) <= _ColorTresh && abs(col.b - _Color2in.b) <= _ColorTresh) ? _Color2out : col;
					col = all(abs(col.r - _Color3in.r) <= _ColorTresh && abs(col.g - _Color3in.g) <= _ColorTresh && abs(col.b - _Color3in.b) <= _ColorTresh) ? _Color3out : col;
					col = all(abs(col.r - _Color4in.r) <= _ColorTresh && abs(col.g - _Color4in.g) <= _ColorTresh && abs(col.b - _Color4in.b) <= _ColorTresh) ? _Color4out : col;
					col = all(abs(col.r - _Color5in.r) <= _ColorTresh && abs(col.g - _Color5in.g) <= _ColorTresh && abs(col.b - _Color5in.b) <= _ColorTresh) ? _Color5out : col;
					col = all(abs(col.r - _Color6in.r) <= _ColorTresh && abs(col.g - _Color6in.g) <= _ColorTresh && abs(col.b - _Color6in.b) <= _ColorTresh) ? _Color6out : col;
					col = all(abs(col.r - _Color7in.r) <= _ColorTresh && abs(col.g - _Color7in.g) <= _ColorTresh && abs(col.b - _Color7in.b) <= _ColorTresh) ? _Color7out : col;
					col = all(abs(col.r - _Color8in.r) <= _ColorTresh && abs(col.g - _Color8in.g) <= _ColorTresh && abs(col.b - _Color8in.b) <= _ColorTresh) ? _Color8out : col;

					half4 mask = tex2D(_MaskTex, i.uv2.xy);
					col.rgb = col.rgb*mask.rgb;
					return col * i.color;
				}
				ENDCG
			}
		}
}
