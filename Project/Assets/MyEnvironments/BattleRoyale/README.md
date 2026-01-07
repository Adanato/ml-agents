# Battle Royale — Team Tactics

A 3v3 (or more) battle royale with specialized team roles fighting for survival.

## Project Status
- [x] Design concept
- [ ] Create base CombatAgent.cs
- [ ] Create DefenderAgent.cs
- [ ] Create MeleeAgent.cs (close combat)
- [ ] Create RangedAgent.cs
- [ ] Build shrinking arena
- [ ] Train with MA-POCA

---

## Design

### Concept
Teams of 3 specialized agents compete in a shrinking arena. Each role has different stats and abilities, requiring teamwork and positioning.

### Team Composition

| Role | Health | Speed | Range | Special Ability |
|------|--------|-------|-------|-----------------|
| **Defender** | 150 | Slow | Short | Shield (blocks damage), Taunt (draws aggro) |
| **Melee** | 100 | Fast | Melee | Dash attack, High burst damage |
| **Ranged** | 75 | Medium | Long | Snipe, Area denial |

### Combat Stats

| Role | Damage | Attack Speed | Optimal Range |
|------|--------|--------------|---------------|
| **Defender** | 10 | Slow | 0-3 units |
| **Melee** | 25 | Fast | 0-2 units |
| **Ranged** | 15 | Medium | 8-15 units |

---

## Observations

### Shared (All Agents)
| # | Observation | Notes |
|---|-------------|-------|
| 1-2 | Own position (x, z) | Normalized to arena |
| 3 | Own health | 0-1 |
| 4-5 | Own velocity | Normalized |
| 6 | Zone distance | Distance to safe zone edge |
| 7 | Ability cooldown | 0-1 |

### Team Awareness (per teammate × 2)
| # | Observation | Notes |
|---|-------------|-------|
| 1-2 | Teammate relative position | |
| 3 | Teammate health | |
| 4 | Teammate role (one-hot) | 3 values |

### Enemy Awareness (per visible enemy × 3)
| # | Observation | Notes |
|---|-------------|-------|
| 1-2 | Enemy relative position | |
| 3 | Enemy health | |
| 4 | Enemy role (one-hot) | 3 values |

### Total: ~30-40 observations depending on visibility

---

## Actions

| Role | Continuous | Discrete |
|------|------------|----------|
| **All** | Move (x, z), Rotate | Attack, Use Ability |
| **Defender** | — | Shield, Taunt |
| **Melee** | — | Dash |
| **Ranged** | Aim direction | Snipe |

---

## Rewards

### Individual
| Event | Reward |
|-------|--------|
| Deal damage | +0.1 per 10 HP |
| Kill enemy | +2.0 |
| Take damage | -0.05 per 10 HP |
| Die | -1.0 |
| Outside zone | -0.1 per second |

### Team
| Event | Reward |
|-------|--------|
| Team kills enemy | +0.5 to all |
| Last team standing | +5.0 to survivors |
| Team eliminated | -2.0 to all |

---

## Arena Mechanics

### Shrinking Zone
```
Phase 1 (0-60s):   Full arena (50×50)
Phase 2 (60-90s):  Shrink to 35×35
Phase 3 (90-120s): Shrink to 20×20
Phase 4 (120s+):   Shrink to 10×10
```

Damage outside zone: 5 HP/sec, increasing each phase

---

## Training Strategy

- **MA-POCA** for team coordination
- **Self-play** between teams
- Curriculum: Start without zone shrink, add later
- ~3M steps for role specialization to emerge

---

## Emergent Behaviors to Watch For
- Defender positioning in front of ranged
- Melee flanking while defender tanks
- Focus fire coordination
- Zone control and positioning
- Retreating to protect low-health teammates

---

## Next Steps
1. [ ] Create arena with shrinking zone system
2. [ ] Implement base combat system (health, damage)
3. [ ] Create 3 agent prefabs with different stats
4. [ ] Add role-specific abilities
5. [ ] Train initial teams
6. [ ] Observe team dynamics and iterate
