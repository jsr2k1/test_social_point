Shader "SocialPoint/Mesh Terrain 4 Splats" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Control ("Control 1, 2, 3, 4 (RGBA)", 2D) = "red" {}		
	_Splat1 ("Texture 1 (R)", 2D) = "white" {}
	_Splat2 ("Texture 2 (G)", 2D) = "white" {}
	_Splat3 ("Texture 3 (B)", 2D) = "white" {}
	_Splat4 ("Texture 4 (A)", 2D) = "white" {}
	
}
	
SubShader {
	Lighting Off
	Tags {
		"SplatCount" = "4"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf Lambert 

fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed4 c;
	c.rgb = s.Albedo; 
	c.a = s.Alpha;
	return c;
}

struct Input {
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat1 : TEXCOORD1;
	float2 uv_Splat2 : TEXCOORD2;
	float2 uv_Splat3 : TEXCOORD3;	
	float2 uv_Splat4 : TEXCOORD4;
};

sampler2D _Control;
sampler2D _Splat1,_Splat2,_Splat3,_Splat4 ;
fixed4 _Color; 

void surf (Input IN, inout SurfaceOutput o) {

	fixed4 splat_control = tex2D (_Control, IN.uv_Control);
	
	fixed3 col;
	
	col  = splat_control.a * splat_control.r * tex2D (_Splat1, IN.uv_Splat1).rgb;
	col += splat_control.a * splat_control.g * tex2D (_Splat2, IN.uv_Splat2).rgb;
	col += splat_control.a * splat_control.b * tex2D (_Splat3, IN.uv_Splat3).rgb;	
	col += (1-splat_control.a) * tex2D (_Splat4, IN.uv_Splat4).rgb;
	
	o.Albedo = col*_Color;
	o.Alpha = 0.0;
}
ENDCG  
}

// Fallback to Diffuse
Fallback "Diffuse"
}
