// Upgrade NOTE: upgraded instancing buffer 'InstanceProperties' to new syntax.

// Toony Colors Free
// (c) 2012,2016 Jean Moreno


// Want more features ? Check out Toony Colors Pro+Mobile 2 !
// http://www.jeanmoreno.com/toonycolorspro/


Shader "Toony Property Block"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		
		//TOONY COLORS RAMP
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,1)) = 0.5
		_RimMax ("Rim Max", Range(0,1)) = 1.0
		
	
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		
		#pragma surface surf ToonyColorsCustom
		#pragma target 2.0
		#pragma glsl

		
		#define _Color_arr InstanceProperties

		UNITY_INSTANCING_BUFFER_START(InstanceProperties)	
		UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
		UNITY_DEFINE_INSTANCED_PROP(float4, _HColor)
		UNITY_DEFINE_INSTANCED_PROP(float4, _SColor)
		UNITY_DEFINE_INSTANCED_PROP(float4, _RimColor)
		
		UNITY_INSTANCING_BUFFER_END(InstanceProperties)


		//================================================================
		// VARIABLES
		
		//fixed4 _Color;
		sampler2D _MainTex;
		
		//fixed4 _RimColor;
		fixed4 __RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		
		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
		};
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		//fixed4 _HColor;
		//fixed4 _SColor;
		fixed4 __HColor;
		fixed4 __SColor;

		sampler2D _Ramp;
		
		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
		
		inline half4 LightingToonyColorsCustom (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			s.Normal = normalize(s.Normal);
			fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
		    __SColor =  UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _SColor);
			__HColor = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _HColor);
			__SColor = lerp(__HColor, __SColor, __SColor.a);	//Shadows intensity through alpha
			ramp = lerp(__SColor.rgb,__HColor.rgb,ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}
		
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			//fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color).rgb;
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex) ;
			
			__RimColor = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _RimColor);

			o.Albedo = mainTex.rgb * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color).rgb;
			o.Alpha = mainTex.a * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color).a;
			
			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (__RimColor.rgb * rim) * __RimColor.a;
		}
		
		ENDCG
	}
	
	Fallback "Diffuse"
	CustomEditor "TCF_MaterialInspector"
}
