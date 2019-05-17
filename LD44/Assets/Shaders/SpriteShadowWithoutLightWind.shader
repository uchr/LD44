Shader "Custom/SpriteShadowWithoutLightWind" {
    Properties {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5

        _WaveTex ("Wave Texture", 2D) = "white" {}
        _HeightTexture ("Height Texture", 2D) = "white" {}
        _WaveScale("Wave Scale", float) = 1.0
        _WindSpeed("Wind Speed", float) = 1.0
        _WindAmp("Wind Amp", float) = 1.0
        _RndFactor("Rnd Factor", float) = 1.0
        _DebugCoef("DebugCoef", float) = 1.0
    }
    SubShader {
        Tags 
        { 
            "Queue"="Geometry"
            "RenderType"="TransparentCutout"
        }
        LOD 200

        Lighting Off
        Cull Off

        CGPROGRAM
        #pragma surface surf Lambert addshadow fullforwardshadows vertex:vert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _Cutoff;

        sampler2D _WaveTex;
        sampler2D _HeightTexture;
        float _WaveScale;
        float _WindSpeed;
        float _WindAmp;
        float _RndFactor;
        float _DebugCoef;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 scaledPos;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float4 pos = mul(unity_ObjectToWorld, v.vertex);
            o.scaledPos = pos / _WaveScale;
            float rnd = tex2Dlod(_WaveTex, float4(o.scaledPos.x, o.scaledPos.z, 0, 0)) - 0.5;
            float heightFactor = tex2Dlod(_HeightTexture, v.texcoord);
            v.vertex.x += _WindAmp * sin(_RndFactor * rnd + _WindSpeed * _Time.y) * heightFactor;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = (tex2D (_MainTex, IN.uv_MainTex) * _Color) * _DebugCoef + tex2D(_WaveTex, float4(IN.scaledPos.xz, 0, 1)) * (1 - _DebugCoef);
            o.Emission = c.rgb;
            o.Alpha = c.a;
            clip(o.Alpha - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
