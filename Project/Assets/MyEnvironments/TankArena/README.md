# Tank Arena

A 1v1 tank battle environment for training AI agents with self-play.

## Project Status
- [x] Design observations, actions, rewards
- [x] Create TankAgent.cs script
- [x] Create training config (self-play)
- [ ] Setup Unity scene (arena + tanks)
- [ ] Create bullet prefab
- [ ] Train initial model
- [ ] Build headless for cluster training

---

## Design

### Observations (12 values)
| # | Observation | Range |
|---|-------------|-------|
| 1-2 | Own position (x, z) | normalized |
| 3 | Own rotation (y) | 0-1 |
| 4-5 | Own velocity (x, z) | normalized |
| 6 | Own health | 0-1 |
| 7-8 | Opponent relative position | normalized |
| 9 | Opponent rotation | 0-1 |
| 10 | Opponent health | 0-1 |
| 11 | Angle to opponent | 0-1 |
| 12 | Fire cooldown remaining | 0-1 |

### Actions
| Type | Index | Description |
|------|-------|-------------|
| Continuous | 0 | Move forward/back (-1 to 1) |
| Continuous | 1 | Rotate left/right (-1 to 1) |
| Discrete | 0 | Fire (0=no, 1=yes) |

### Rewards
| Event | Reward |
|-------|--------|
| Hit enemy | +1.0 |
| Kill enemy | +5.0 |
| Get hit | -0.5 |
| Die | -1.0 |
| Per step | -0.001 |

---

## Unity Scene Setup

### 1. Create Arena
- Floor: 20×20 plane
- Walls: 4 cubes around edges
- Add colliders to all

### 2. Create Tank Prefab
```
Tank (GameObject)
├── Body (Cube with Rigidbody)
├── Turret (child cube, optional)
└── FirePoint (empty transform at barrel tip)

Components on Tank:
- Rigidbody (constraints: freeze Y position, freeze X/Z rotation)
- TankAgent.cs
- Behavior Parameters:
  - Behavior Name: TankArena
  - Vector Observation: 12
  - Continuous Actions: 2
  - Discrete Branches: [2]
- Decision Requester (Decision Period: 5)
```

### 3. Create Bullet Prefab
- Small sphere (scale 0.2)
- Rigidbody (no gravity, or low gravity)
- Collider (is trigger optional)

### 4. Link References
- Each tank's `opponent` field → other tank
- Each tank's `bulletPrefab` → bullet prefab
- Each tank's `firePoint` → its FirePoint transform

---

## Training Commands

### Local Training (Editor)
```bash
.venv/bin/mlagents-learn config/ppo/TankArena.yaml --run-id=tank_v1 --force
```

### Headless Training (Multi-Instance)
```bash
.venv/bin/mlagents-learn config/ppo/TankArena.yaml \
  --run-id=tank_headless \
  --env=./builds/TankArena/TankArena.x86_64 \
  --num-envs=8 \
  --no-graphics
```

---

## Next Steps
1. [ ] Complete Unity scene setup
2. [ ] Test with Heuristic mode (keyboard controls)
3. [ ] Train initial model locally
4. [ ] Build headless executable
5. [ ] Scale training on Slurm cluster
6. [ ] Add raycasts for obstacle detection
7. [ ] Add visual variety (different arena layouts)
