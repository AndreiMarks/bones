Shader "Shapes/Basic Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Tint Color", Color) = (1,1,1,1)
		_BlendOp ("Blend Op", Int) = 0
		_SrcMode ("Source Blend", Int) = 5
     	_DstMode ("Destination Blend", Int) = 10
	}
	
	SubShader {
	
		Name "Diffuse"
		BlendOp [_BlendOp]
		Blend [_SrcMode] [_DstMode]
	
		Tags { "RenderType"="Opaque" "Queue" = "Transparent" "ShapesGenerated"="True"}

		ZWrite On
		LOD 200
	
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color;

		struct Input {
			float2 uv_MainTex;
			float4 color:COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * IN.color.rgb * _Color.rgb;
			o.Alpha = c.a * IN.color.a * _Color.a;
		}
		ENDCG
		
	} 
	FallBack "Diffuse"
}
