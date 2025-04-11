// MIT

shader_type spatial;

uniform float min_height = 0.0;
uniform float max_height = 1.0;

uniform sampler2D grass_tex : source_color;
uniform sampler2D grass_normal_tex : hint_normal;
uniform vec2 grass_scale;

uniform sampler2D dirt_tex : source_color;
uniform sampler2D dirt_normal_tex : hint_normal;
uniform vec2 dirt_scale;

uniform sampler2D crabgrass_tex : source_color;
uniform sampler2D crabgrass_normal_tex : hint_normal;
uniform vec2 crabgrass_scale;

uniform sampler2D mask_tex : source_color;
uniform vec2 mask_scale;

varying float height_val;
varying vec3 normal;
varying vec3 world_pos;

void vertex()
{
    height_val = VERTEX.y;
    normal = NORMAL;
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment()
{
    vec2 grass_uv = world_pos.xz * grass_scale;
    vec2 dirt_uv = world_pos.xz * dirt_scale;
    vec2 crabgrass_uv = world_pos.xz * crabgrass_scale;
    vec2 mask_uv = world_pos.xz * mask_scale;

    vec3 dirt = texture(dirt_tex, dirt_uv).rgb;
    vec3 grass = texture(grass_tex, grass_uv).rgb;
    vec3 crabgrass = texture(crabgrass_tex, crabgrass_uv).rgb;

    vec3 dirt_normal = texture(dirt_normal_tex, dirt_uv).rgb * 2.0 - 1.0;
    vec3 grass_normal = texture(grass_normal_tex, grass_uv).rgb * 2.0 - 1.0;
    vec3 crabgrass_normal = texture(crabgrass_normal_tex, crabgrass_uv).rgb * 2.0 - 1.0;

    dirt_normal.z = sqrt(max(1.0 - dot(dirt_normal.xy, dirt_normal.xy), 0.0));
    vec3 dirt_normal_ws = normalize(dirt_normal.x * TANGENT + dirt_normal.y * BINORMAL + dirt_normal.z * NORMAL);

    grass_normal.z = sqrt(max(1.0 - dot(grass_normal.xy, grass_normal.xy), 0.0));
    vec3 grass_normal_ws = normalize(grass_normal.x * TANGENT + grass_normal.y * BINORMAL + grass_normal.z * NORMAL);

    crabgrass_normal.z = sqrt(max(1.0 - dot(crabgrass_normal.xy, crabgrass_normal.xy), 0.0));
    vec3 crabgrass_normal_ws = normalize(crabgrass_normal.x * TANGENT + crabgrass_normal.y * BINORMAL + crabgrass_normal.z * NORMAL);

    float height_blend = smoothstep(min_height, max_height, height_val);
    float mask_value = texture(mask_tex, mask_uv).r;
    vec3 blend_color = mix(dirt, mix(grass, crabgrass, mask_value), height_blend);
    vec3 blend_normal = normalize(mix(dirt_normal_ws, mix(grass_normal_ws, crabgrass_normal_ws, mask_value), height_blend));

    ALBEDO = blend_color;
    NORMAL = blend_normal;
}

