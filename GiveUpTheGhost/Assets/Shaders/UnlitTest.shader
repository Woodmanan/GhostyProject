Shader "Unlit/UnlitTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Off("Sample Offset", float) = 0
        _BThresh("Black Threshold", float) = .1
        _SampleCount("Num Samples", float) = 8
        _Col("OOB Color", float) = (0,0,0,0)
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Cull front
		LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            uniform float _ActualOff;
            uniform float _Off;
            uniform float _BThresh;
            uniform float _SampleCount;
            uniform float4 _Col;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            bool isBlack(float4 col)
            {
                float add = col.x + col.y + col.z;
                return add < _BThresh;
            }
            
            float4 sample(float2 uv, float xOff, float yOff)
            {
                float2 update = float2(uv.x + xOff, uv.y + yOff);
                float outBounds = (step(update.x, 0) + step(1, update.x));
                outBounds = outBounds + step(update.y, 0) + step(1, update.y);
                float inBounds = 1 - outBounds;
                return _Col * outBounds + tex2D(_MainTex, update) * inBounds;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float add = 1;
                fixed4 col = sample(i.uv, 0, 0);
                
                if (_SampleCount == 0)
                {
                    return col;
                }
                
                for (float angle = 0; angle < 360; angle += 360 / _SampleCount)
                {
                    float4 next = sample(i.uv, cos(angle) * _Off, sin(angle) * _Off);
                    float addVal = next.x + next.y + next.z;
                    float shouldAdd = step(_BThresh, addVal);
                    add += shouldAdd;
                    col += next;
                }
                
                
                return col / add;
            }
            ENDCG
        }
    }
}
