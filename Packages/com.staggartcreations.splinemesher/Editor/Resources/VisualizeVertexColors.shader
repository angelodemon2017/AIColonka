Shader "Hidden/VisualizeVertexColors"
{
	Properties
	{
		[Enum(Red,0,Green,1,Blue,2,Alpha,3,All,4)] _Channel ("Color Channel", Float) = 0
		[Toggle] _Transparent ("Transparent", Float) = 0
	}
	
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Transparent" 
			"RenderQueue"="Transparent" 
			//"LightMode" = "Forward"
		}
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uint _Channel;
			bool _Transparent;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float alpha = _Transparent ? i.color.a : 1.0;

				//RGB(A)
				if(_Channel == 4) return float4(i.color.rgb, alpha);

				//Specific channel
				float value = i.color[_Channel];
				
				return float4(value.xxx, alpha);
			}
			ENDCG
		}
	}
}