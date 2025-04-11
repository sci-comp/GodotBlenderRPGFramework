shader_type spatial;

/*
Shader: World-Space Slope-Based Triplanar Blend
===========================================================
- Applies triplanar textures for foreground and background layers.
- Blends based on slope in world space, ensuring upward-facing surfaces use the foreground texture.
- Independent of object orientation and camera view.
*/

uniform float slope_threshold : hint_range(0.0, 1.0) = 0.5;
uniform float blend_fade : hint_range(0.0, 0.1) = 0.1;

uniform vec3 uv_scale_fg = vec3(1.0);
uniform vec3 uv_scale_bg = vec3(1.0);

uniform sampler2D albedo_texture_fg : source_color, filter_linear_mipmap, repeat_enable;
uniform sampler2D normal_texture_fg : hint_normal, filter_linear_mipmap, repeat_enable;

uniform sampler2D albedo_texture_bg : source_color, filter_linear_mipmap, repeat_enable;
uniform sampler2D normal_texture_bg : hint_normal, filter_linear_mipmap, repeat_enable;

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

    float upward_facing = dot(normalize(world_normal), vec3(0.0, 1.0, 0.0));
    float blend_mask = smoothstep(slope_threshold - blend_fade, slope_threshold + blend_fade, upward_facing);

    ALBEDO = mix(albedo_bg, albedo_fg, blend_mask);
    NORMAL_MAP = mix(normal_bg, normal_fg, blend_mask);
}
