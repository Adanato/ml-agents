# Capture the Flag

Classic 2-team CTF with strategic offense and defense.

## Project Status
- [x] Design concept
- [ ] Create CTFAgent.cs
- [ ] Build arena with bases
- [ ] Implement flag mechanics
- [ ] Train with MA-POCA + self-play

---

## Design

### Concept
Two teams compete to steal the enemy flag and return it to their base while defending their own flag.

### Teams
- 3-5 agents per team
- Symmetric arena
- Respawn on death

### Win Condition
- First to 3 captures, or most captures after time limit

---

## Observations
| # | Observation |
|---|-------------|
| 1-3 | Own position, health, has_flag |
| 4-6 | Own flag status (at base, carried, dropped position) |
| 7-9 | Enemy flag status |
| 10+ | Teammate/enemy positions (raycast or direct) |

## Actions
| Type | Description |
|------|-------------|
| Continuous | Move, rotate |
| Discrete | Grab flag, Drop flag, Tag enemy |

## Rewards
| Event | Reward |
|-------|--------|
| Capture flag | +10.0 |
| Return own flag | +3.0 |
| Tag flag carrier | +2.0 |
| Kill enemy | +1.0 |
| Die | -0.5 |
| Holding flag (per step) | +0.01 |

---

## Training Strategy
- **MA-POCA** for team coordination
- **Self-play** for balanced offensive/defensive play
- Curriculum: Start with no respawn, add later

---

## Next Steps
1. [ ] Create symmetric arena with 2 bases
2. [ ] Implement flag pickup/drop/capture
3. [ ] Add respawn system
4. [ ] Train initial model
