Shader "Custom/SpriteShadowWithoutLightWind" {
    Properties {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _NosiyTex ("Sprite Texture", 2D) = "white" {}
        _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Lighting Off
        Tags 
        { 
            "Queue"="Geometry"
            "RenderType"="TransparentCutout"
        }
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Lambert addshadow fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NosiyTex;
        fixed4 _Color;
        fixed _Cutoff;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void vert(inout appdata_full v)
        {
            float3 w = mul(unity_ObjectToWorld, v.vertex).xyz;
            float4 c = tex2Dlod(_NosiyTex, float4(0.02  * w.x, 0.02 * w.z, 0, 1));
            float s = sin(20 * ( (c.x + c.y) + _Time));
            v.vertex.x += 0.1 * s * clamp(v.vertex.y, 0, 1);
        }

        void surf (Input IN, inout SurfaceOutput o) {
            //float4 c = tex2Dlod(_NosiyTex, float4(0.02  * IN.worldPos.x, 0.02 * IN.worldPos.z, 0, 1));
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = c.rgb;
            o.Alpha = c.a;
            clip(o.Alpha - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
