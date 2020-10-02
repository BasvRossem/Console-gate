// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/ColorInverter"
{
	Properties
	{
		_Color("Tint Color", Color) = (1,1,1,1)
	}

		SubShader
	{
		// Reder last
		Tags { "Queue" = "Transparent" }


		Pass
		{
			// Prevent color from objects behind
			ColorMask 0
		}


		// We know this works
		// But WHY???
		Blend OneMinusDstColor OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex: POSITION;
				float4 color : COLOR;
			};

			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR0;
			};

			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.color = _Color;
				return o;
			}

			half4 frag(fragmentInput i) : COLOR
			{
				return i.color;
			}

			ENDCG
		}


	}
}