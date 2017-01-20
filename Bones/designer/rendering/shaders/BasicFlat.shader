Shader "Shapes/Basic Flat" {
	Properties {
		_MainTex ("Main (RGB)", 2D) = "white" {}
		_BlendOp ("Blend Op", Int) = 0
		_SrcMode ("Source Blend", Int) = 5
     	_DstMode ("Destination Blend", Int) = 10
	}
	
	SubShader {	
		Name "Flat"
		BlendOp [_BlendOp]
		Blend [_SrcMode] [_DstMode]
		
		
     
		ZWrite On
		Cull Off
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "ShapesGenerated"="True"}
		Pass
		{
			BindChannels {
				Bind "Vertex", vertex
				Bind "TexCoord", texcoord
				Bind "Color", color
			}
			
			SetTexture[_MainTex]  {Combine texture * primary}
		}
	 	
	} 
	
}

