// MIT

shader_type spatial;

render_mode unshaded, blend_add, cull_disabled, fog_disabled;

uniform float detail_strength = 3.0;
uniform float scroll_speed = 1.2;
uniform float fire_height = 1.0;
uniform float fire_shape = 1.5;
uniform float fire_thickness = 0.55;
uniform float fire_sharpness = 1.0;
uniform float intensity = 1.0;

uniform int noise_octaves = 6;
uniform float noise_lacunarity = 3.0;
uniform float noise_gain = 0.5;
uniform float noise_amplitude = 1.0;
uniform float noise_frequency = 1.5;

varying vec3 instance_pos;
uniform int pixelization = 0;

float hash(vec2 p) {
    p = fract(p * 0.3183099 + vec2(0.1, 0.1));
    p *= 17.0;
    return fract(p.x * p.y * (p.x + p.y));
}

float noise(vec2 x) {
    vec2 p = floor(x);
    vec2 f = fract(x);
    f = f * f * (3.0 - 2.0 * f);

    return mix(
        mix(hash(p), hash(p + vec2(1.0, 0.0)), f.x),
        mix(hash(p + vec2(0.0, 1.0)), hash(p + vec2(1.0, 1.0)), f.x),
        f.y
    );
}

float fbm(vec2 p) {
    float total = 0.0;
    float amplitude = noise_amplitude;
    float frequency = noise_frequency;

    for (int i = 0; i < noise_octaves; i++) {
        total += noise(p * frequency) * amplitude;
        frequency *= noise_lacunarity;
        amplitude *= noise_gain;
    }

    return total * 0.5;
}

void fragment() {
    vec2 uv = UV;

    vec2 modified_uv = vec2(mod(-uv.x, 1.0) - 0.5, -uv.y + 0.84);
    modified_uv = (pixelization > 0) ? floor(modified_uv * float(pixelization)) / float(pixelization) : modified_uv;

    float scroll = scroll_speed * detail_strength * TIME;
    float noise_val = fbm(detail_strength * modified_uv - vec2(instance_pos.x + instance_pos.y, scroll));

    float mod_uv_len = length(
        modified_uv * vec2(
            (1.0 / fire_thickness) + modified_uv.y * fire_shape,
            1.0 / fire_height
        )
    );

    float fire_intensity = intensity - 16.0 * fire_sharpness * pow(
        max(0.0, mod_uv_len - noise_val * max(0.0, modified_uv.y + 0.25)),
        1.2
    );

    float fire_intensity1 = clamp(
        noise_val * fire_intensity * (1.5 - pow(uv.y, 14.0)),
        0.0,
        1.0
    );

    vec3 fire_color = vec3(
        1.5 * fire_intensity1,
        1.5 * pow(fire_intensity1, 3.0),
        pow(fire_intensity1, 6.0)
    );

    float alpha = fire_intensity * (1.0 - pow(uv.y, 3.0));
    vec4 final_color = vec4(
        mix(vec3(0.0), fire_color, alpha),
        alpha
    );

    ALBEDO = final_color.rgb;
    ALPHA = final_color.a;
}

void vertex() {
    instance_pos = MODEL_MATRIX[3].xyz;
    vec3 local_up = MODEL_MATRIX[1].xyz;

	// Right: local_transform.basis.y CROSS to_world(view_space.forward)
    vec4 ax = vec4(normalize(cross(local_up, INV_VIEW_MATRIX[2].xyz)), 0.0);
	// Up: local_transform.basis.y
    vec4 ay = vec4(local_up.xyz, 0.0);
	// Forward: to_world(view_space.right) CROSS local_transform.basis.y
    vec4 az = vec4(normalize(cross(INV_VIEW_MATRIX[0].xyz, local_up)), 0.0);

    MODELVIEW_MATRIX = VIEW_MATRIX * mat4(ax, ay, az, MODEL_MATRIX[3]);
    MODELVIEW_NORMAL_MATRIX = mat3(MODELVIEW_MATRIX);
}
