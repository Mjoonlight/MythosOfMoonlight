﻿sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 ScreenBasic(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    colour.rgb = float4(128, 128, 0, 255);
    return colour * sampleColor;
}

technique Technique1
{
    pass ScreenBasic
    {
        PixelShader = compile ps_2_0 ScreenBasic();
    }
}