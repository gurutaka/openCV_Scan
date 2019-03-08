Shader "Custom/FishShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Wavelength ("Wavelength", Range(0,1.2)) = 0.7
        _Frequency ("Frequency", Range(0,50)) = 30
        _Speed ("Speed", Range(80,120)) = 100
        _Color ("Color", Color) = (0, 0, 0, 1)
    }
    SubShader{
        Tags{ "RenderType" = "Opaque" }
        CGPROGRAM
 
        #pragma surface surf SimpleLambert vertex:vert
 
        struct Input {
            float2 uv_MainTex;
        };
 
        sampler2D _MainTex;
        float _Wavelength;
        float _Frequency;
        float _Speed;
        fixed4 _Color;
        
         void vert(inout appdata_full v, out Input o )
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            //float amp =  0.01*sin(_Time*100 + v.vertex.z * 100);
            float amp = pow(v.vertex.z - 0.05 , 2) * _Wavelength * sin(_Time*_Speed  + v.vertex.z * _Frequency);
            v.vertex.xyz = float3(v.vertex.x + amp, v.vertex.y, v.vertex.z);            
        }
        
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            //o.Albedo = _Color;
        }
 
        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot(s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 1.5);
            c.a = s.Alpha;
            return c;
        }
        ENDCG
    }
    Fallback "Diffuse"
}