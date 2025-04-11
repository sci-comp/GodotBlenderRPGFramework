shader_type spatial;

/*
Shader: World-Space Mask-Based Triplanar Blend
===========================================================
- Applies triplanar textures for foreground and background layers.
- Uses a binary mask texture to control where the foreground texture is applied.
- Independent of object orientation and position.
*/

uniform vec3 uv_scale_fg = vec3(1.0);
uniform vec3 uv_scale_bg = vec3(1.0);

uniform sampler2D albedo_texture_fg : source_color, filter_linear_mipmap, repeat_enable;
uniform sampler2D normal_texture_fg : hint_normal, filter_linear_mipmap, repeat_enable;

uniform sampler2D albedo_texture_bg : source_color, filter_linear_mipmap, repeat_enable;
uniform sampler2D normal_texture_bg : hint_normal, filter_linear_mipmap, repeat_enable;

uniform sampler2D mask_texture : source_color, filter_linear, repeat_enable;

uniform float mask_threshold_min : hint_range(0.0, 1.0) = 0.2;
uniform float mask_threshold_max : hint_range(0.0, 1.0) = 0.8;

varying vec3 world_pos;
varying vec3 world_normal;

void vertex() {
    world_normal = normalize((MODEL_MATRIX * vec4(NORMAL, 0.0)).xyz);
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

vec3 triplanar_map(sampler2D tex, vec3 pos, vec3 scale) {
    vec3 abs_normal = abs(world_normal);
    vec3 blend_weights = abs_normal / (abs_normal.x + abs_normal.y + abs_normal.z + 1e-5);

    vec2 uv_x = pos.yz * scale.yz;
    vec2 uv_y = pos.xz * scale.xz;
    vec2 uv_z = pos.xy * scale.zx;

    return texture(tex, uv_x).rgb * blend_weights.x +
           texture(tex, uv_y).rgb * blend_weights.y +
           texture(tex, uv_z).rgb * blend_weights.z;
}

void fragment() {
    vec3 albedo_fg = triplanar_map(albedo_texture_fg, world_pos, uv_scale_fg);
    vec3 normal_fg = triplanar_map(normal_texture_fg, world_pos, uv_scale_fg);

    vec3 albedo_bg = triplanar_map(albedo_texture_bg, world_pos, uv_scale_bg);
    vec3 normal_bg = triplanar_map(normal_texture_bg, world_pos, uv_scale_bg);

    float mask_value = texture(mask_texture, UV).r;
    float blend_mask = smoothstep(mask_threshold_min, mask_threshold_max, mask_value);

    ALBEDO = mix(albedo_bg, albedo_fg, blend_mask);
    NORMAL_MAP = mix(normal_bg, normal_fg, blend_mask);
}
