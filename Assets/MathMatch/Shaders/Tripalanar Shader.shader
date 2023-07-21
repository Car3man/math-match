Shader "Custom/Tripalanar Shader"
{
    Properties
    {
        _TopColor ("TopColor", Color) = (1,1,1,1)
        _SideColor ("SideColor", Color) = (0.9,0.9,0.9,1)
        _FrontColor ("FrontColor", Color) = (0.8,0.8,0.8,1)
        _Shadow ("Shadow", float) = 0.5
        _FogColor ("FogColor", Color) = (1,1,1,1)
        _FogMinHeight ("FogMinHeight", float) = -1.0
        _FogMaxHeight ("FogMaxHeight", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };
        
        fixed4 _TopColor;
        fixed4 _SideColor;
        fixed4 _FrontColor;
        float _Shadow;
        fixed4 _FogColor;
        float _FogMinHeight;
        float _FogMaxHeight;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed3 top = abs(dot(IN.worldNormal, fixed3(0,1,0))) * _TopColor;
            fixed3 side = abs(dot(IN.worldNormal, fixed3(1,0,0))) * _SideColor;
            fixed3 front = abs(dot(IN.worldNormal, fixed3(0,0,1))) * _FrontColor;
            fixed3 emission = top + side + front;
            float fogPos = clamp((_FogMaxHeight - IN.worldPos.y) / (_FogMaxHeight - _FogMinHeight), 0, 1);
            
            emission = lerp(emission, _FogColor, fogPos);
            o.Emission = emission;
            o.Albedo = emission * _Shadow;
            o.Metallic = 0;
            o.Smoothness = 0.5;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}