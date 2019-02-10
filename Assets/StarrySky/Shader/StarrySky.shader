// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StarrySky" {
	Properties {
		_Resolution ("Resolution", Vector) = (0, 0, 0, 0)
		_FrequencyVariation ("Frequency Variation", Range(0.5, 1.0)) = 1.3
		_Origin ("Origin", Vector) = (0, 0, 0, 0)
		_Sparsity ("Sparsity", Range(0.1, 1.0)) = 0.5
		_Brightness ("Brightness", Range(0.001, 0.008)) = 0.0018
		_DistFading ("Distance Fading", Range(0.1, 1.0)) = 0.68
		_StepSize ("Step Size", Range(0.1, 1.0)) = 0.2
		_Zoom ("Zoom", Range(1, 16)) = 8
		_Offset ("Offset", Vector) = (0, 0, 0, 0)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#define I 8
		#define J 20
		uniform float4 _Resolution;
		uniform float _FrequencyVariation;
		uniform float4 _Origin;
		uniform float _Sparsity;
		uniform float _Brightness;
		uniform float _DistFading;
		uniform float _StepSize;
		uniform float _Zoom;
		uniform float4 _Offset;
        struct v2f
        {
        	float4 pos : POSITION;
            float2 tex : TEXCOORD0;
        };
        v2f vert (float4 pos : POSITION, float2 tex : TEXCOORD0)
        {
        	v2f o;
            o.pos = UnityObjectToClipPos(pos);
            o.tex = tex;
            return o;
       	}
       	float4 frag (v2f i) : COLOR
       	{
			float2 uv = i.tex * (1.0 / _Resolution.xy) - 0.5;
			uv.y *= _Resolution.y * (1.0 / _Resolution.x);
 
			float3 dir = float3(uv * _Zoom, 1.0);
			dir.xy += _Offset.xy;
 
			float s = 0.1;
			float fade = 0.01;
			float3 final = float3(0.0, 0.0, 0.0);

			for (int i = 0; i < I; ++i)
			{
				float3 p = _Origin.xyz + dir * (s * 0.5);
				float3 vfreq = float3(_FrequencyVariation, _FrequencyVariation, _FrequencyVariation);
				float3 vfreq2x = float3(_FrequencyVariation * 2.0, _FrequencyVariation * 2.0, _FrequencyVariation * 2.0);
				p = abs(vfreq - fmod(p, vfreq2x));
 
				float prevlen = 0.0;
				float a = 0.0;
				for (int j = 0; j < J; ++j)
				{
					p = abs(p);
					p = p * (1.0 / dot(p, p)) - _Sparsity;
					float len = length(p);
					a += abs(len - prevlen);
					prevlen = len;
				}

				a *= a * a;

				final += (float3(s, s*s, s*s*s) * a * _Brightness + 1.0) * fade;
				fade *= _DistFading;
				s += _StepSize;
			}
			return float4(final, 1);
       	}
	ENDCG
	SubShader {
		Cull Off
		Blend Off
	  	Fog { Mode off }
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
	FallBack Off
}