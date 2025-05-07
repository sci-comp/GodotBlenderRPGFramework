extends CharacterBody3D

@export_category("Character")
@export var base_speed : float = 5.0
@export var sprint_speed : float = 8.0
@export var jump_velocity : float = 4.5
@export var mouse_sensitivity : float = 0.1

@export_group("Nodes")
@export var HEAD : Node3D

@export_group("Controls")
@export var JUMP : String = "jump"
@export var LEFT : String = "move_left"
@export var RIGHT : String = "move_right"
@export var FORWARD : String = "move_forward"
@export var BACKWARD : String = "move_backward"
@export var PAUSE : String = "start"
@export var SPRINT : String = "sprint"

# Get the gravity from the project settings
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")
var speed = base_speed

func _ready():
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	
	# If controller was rotated, redirect to head
	HEAD.rotation.y = rotation.y
	rotation.y = 0

func _physics_process(delta):
	# Add gravity
	if not is_on_floor():
		velocity.y -= gravity * delta
	
	# Handle jump
	if Input.is_action_just_pressed(JUMP) and is_on_floor():
		velocity.y = jump_velocity
	
	# Handle sprint
	speed = sprint_speed if Input.is_action_pressed(SPRINT) else base_speed
	
	# Get input direction and handle movement
	var input_dir = Input.get_vector(LEFT, RIGHT, FORWARD, BACKWARD)
	var direction = input_dir.rotated(-HEAD.rotation.y)
	direction = Vector3(direction.x, 0, direction.y).normalized()
	
	if direction:
		velocity.x = direction.x * speed
		velocity.z = direction.z * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
		velocity.z = move_toward(velocity.z, 0, speed)
	
	move_and_slide()

func _process(_delta):
	if Input.is_action_just_pressed(PAUSE):
		toggle_mouse_capture()
	
	HEAD.rotation.x = clamp(HEAD.rotation.x, deg_to_rad(-90), deg_to_rad(90))

func _unhandled_input(event):
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		HEAD.rotation_degrees.y -= event.relative.x * mouse_sensitivity
		HEAD.rotation_degrees.x -= event.relative.y * mouse_sensitivity

func toggle_mouse_capture():
	if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	else:
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
