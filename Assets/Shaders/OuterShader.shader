// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/OuterMaterialShaderPlane"
{
	Properties
	{        
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_MainOffset ("Main offset", Vector) =(0,0,0,0)
		_Color ("Tint", Color) = (1,1,1,1)
		_HeroPos ("_HeroPos", Vector) = (0,0,0,0)
		_ColorSample ("SmokeSample", Color) = (1,1,1,1)
		_MaxReveal("Porog", float) =0.0
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _NoiseMask("NoiseMask", 2D) = "white" {}
        _IntensityAndScrolling("Intensity (XY), Scrolling (ZW)", Vector) = (0.1,0.1,0.1,0.1)
        _reveal_distance("reveal distance", float)=5

	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent+10"
			"IgnoreProjector"="True" 
			"RenderType"="Opaque" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite on 
		ZTest LEqual
		Fog { Mode Off }
         Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha:fade
			#pragma multi_compile DUMMY PIXELSNAP_ON

 			#include "UnityCG.cginc"
	

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				half2 texcoord2  : TEXCOORD1;
				float4 world	: POSITION1;
			};
			
			fixed4 _Color;
			fixed4 _MainOffset;
			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			sampler2D _NoiseTex;
			fixed4 _NoiseTex_ST;
			sampler2D _NoiseMask;
			fixed4 _NoiseMask_ST;
			sampler2D _SmokeSample;


			fixed4 _IntensityAndScrolling;

			fixed4 _ColorSample;
			fixed4 _HeroPos;
			float _MaxReveal;
			float _reveal_distance;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.texcoord2 = TRANSFORM_TEX(IN.texcoord2, _NoiseTex);
                OUT	.texcoord2 += _Time.yy * _IntensityAndScrolling.zw;	


				OUT.world = mul(unity_ObjectToWorld, IN.vertex);

//				OUT.texcoord = IN.texcoord;
				OUT.color =  _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}




			fixed4 sm_diff(float2 coord,float4 w_pos,float2 noised_coord,fixed4 noisedColor)
			{
				half2 dist = coord;
				dist.xy+=_MainOffset.xy;

				fixed4 original = tex2D(_MainTex, dist);
				float koef = (clamp((abs(w_pos.x-_HeroPos.x)-_reveal_distance),0,_MaxReveal)+clamp(abs(w_pos.z-_HeroPos.z)-_reveal_distance,0,_MaxReveal));
				original.a=(1-koef/_MaxReveal);
				if (koef>0)
					original.a*=noisedColor.a;

				return original;
			}


			fixed4 frag(v2f IN) : SV_Target
			{
				float4 noiseTex = tex2D(_NoiseTex, IN.texcoord2);
                float2 offset = (noiseTex.rg * 2 - 1) * _IntensityAndScrolling.rg;
                fixed4 clrNoise =tex2D(_NoiseMask,IN.texcoord + offset);
                float2 uvNoise = IN.texcoord;
                //float4 mainTex = tex2D(_MainTex, uvNoise);

//				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				fixed4 c=sm_diff(IN.texcoord,IN.world,uvNoise,clrNoise);
//				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
