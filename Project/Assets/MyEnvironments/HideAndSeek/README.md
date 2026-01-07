# Hide and Seek

A multi-agent asymmetric game where Hiders try to evade Seekers in procedural environments.

## Project Status
- [x] Design concept
- [ ] Create HiderAgent.cs
- [ ] Create SeekerAgent.cs
- [ ] Build procedural arena generator
- [ ] Add moveable objects (boxes, ramps)
- [ ] Train with MA-POCA

---

## Design

### Concept
Inspired by OpenAI's emergent behavior research. Hiders get a "preparation phase" to build defenses, then Seekers are released.

### Teams
| Role | Count | Goal |
|------|-------|------|
| **Hiders** | 2-3 | Avoid being seen until time runs out |
| **Seekers** | 1-2 | Tag all hiders before time runs out |

### Phases
1. **Prep Phase** (30 sec): Only Hiders can move. Build defenses.
2. **Seek Phase** (60 sec): Seekers released, hunt begins.

### Observations
| Agent | Observations |
|-------|--------------|
| **Both** | Own position, rotation, velocity |
| **Both** | Raycast sensors (walls, objects, other agents) |
| **Seeker** | Number of hiders remaining |
| **Hider** | Time remaining, distance to nearest seeker |

### Actions
| Type | Description |
|------|-------------|
| Continuous | Move forward/back, strafe, rotate |
| Discrete | Grab object, release object, lock box |

### Rewards
| Event | Hider | Seeker |
|-------|-------|--------|
| Time surviving | +0.01/step | — |
| Getting tagged | -1.0 | +1.0 |
| All hiders found | — | +5.0 |
| Time runs out | +2.0 each | -1.0 each |

### Emergent Behaviors to Watch For
- Hiders blocking doors
- Hiders building "forts" with boxes
- Seekers using ramps to climb over walls
- Cooperation between teammates

---

## Training Strategy
- **MA-POCA** for team coordination
- **Curriculum**: Start with simple rooms, add complexity
- 2M+ steps for emergent behaviors to develop

---

## Next Steps
1. [ ] Create simple arena with moveable boxes
2. [ ] Implement grab/release mechanics
3. [ ] Add line-of-sight detection for tagging
4. [ ] Train initial model
5. [ ] Add procedural arena generation
