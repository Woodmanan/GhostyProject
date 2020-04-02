Shader "Unlit/UnlitTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ActualOff("Actual Offset", float) = 0
        _HeightMult("Height Multiplier", float) = 0
        _Offset("Offset Multiplier", float) = 0
        _OffAmt("Offset Amount", float) = 0
        _GrnAmt("Green Amount", float) = 0
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
            uniform float _HeightMult;
            uniform float _Offset;
            uniform float _OffAmt;
            uniform float _GrnAmt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float sideOffset = sin(i.uv.y * _HeightMult + _OffAmt) * _Offset;
                float2 uv = float2(i.uv.x * _ActualOff + sideOffset,  i.uv.y);
                fixed4 col = tex2D(_MainTex, uv);
                
                float green = 1 - _GrnAmt;
                
                float4 greenCol = float4(green, 1, green, 1);
                col = col * greenCol;
                
                float val = (step(uv.x, 0) + step(1, uv.x));//* (step(uv.y, 0) + step(1, uv.y)); 
                float val2 = 1-val;       
                float4 blank = float4(0,0,0,0);
                
                return col * val2 + blank * val;
            }
            ENDCG
        }
    }
}
