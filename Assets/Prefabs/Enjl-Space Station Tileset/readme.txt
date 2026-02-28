Use of the sample fragment shader:
- Use the tileset texture as the iChannel0 GLSL input.
- Choose a noise texture as the iChannel1 GLSL input.
- Make sure the iTime and iResolution inputs are supplied.
- Uncomment/Comment the marked lines
- The shader can be found at https://www.shadertoy.com/view/NsfXDS

Should the link not work, there is a backup below.
You may also use whichever other shader you like, or opt to use static colours. I just really like the smooth animation!
Enjoy :)

##############################

// Sample shader for Space Station Tileset fill.

// Use the tile's texture for iChannel0. iChannel1 uses noise.

const vec4 tint1 = vec4(0.2, 0.7, 0.9, 1.0);
const vec4 tint2 = vec4(0.5, 0.2, 0.2, 1.0);
const float speedScale = 0.1f;

//Less than conditional
float lt(float x, float y) 
{
  return max(sign(y - x), 0.0);
}

//Greater or equal conditional
float ge(float x, float y) 
{
  return 1.0 - lt(x, y);
}

//Greater than conditional
float gt(float x, float y)
{
	return max(sign(x - y), 0.0);
}

//Less or equal conditional
float le(float x, float y) 
{
  return 1.0 - gt(x, y);
}

float and(float a, float b)
{
	return a*b;
}

float eq(float a, float b)
{
	return and(ge(a,b), le(a,b));
}


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = fragCoord/iResolution.xy;

    vec4 col = texture(iChannel0, uv.xy);

    vec2 uv2 = vec2(mod(uv.x + iTime * 0.5 * speedScale, 1.0), mod(uv.y + iTime * 0.6 * speedScale, 1.0));
    vec2 uv3 = vec2(mod(uv.x + iTime * -0.3 * speedScale, 1.0), mod(uv.y + iTime * -0.4 * speedScale, 1.0));

    vec4 colb = texture(iChannel1, uv2) * tint1 + texture(iChannel1, uv3) * tint2;

    float isR = eq(col.r, 1.0);
    float isG = eq(col.g, 1.0);
    float isB = eq(col.b, 1.0);
    float isnR = eq(col.r, 0.0);
    float isnG = eq(col.g, 0.0);
    float isnB = eq(col.b, 0.0);
    float red = and(and(isR, isnG), isnB);
    float green = and(and(isG, isnR), isnB);
    float blue = and(and(isB, isnG), isnR);


    // Uncomment this when the proper input is applied
    // col = mix(col, colb, red);
    // col = mix(col, colb * 0.75, green);
    // col = mix(col, colb * 0.4, blue);
    
    // Comment this when the proper input is applied
    col += colb;

    // Output to screen
    fragColor = col;
}