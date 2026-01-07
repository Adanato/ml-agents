# Ecosystem Simulation

A multi-species simulation where agents evolve survival strategies through competition and cooperation.

## Project Status
- [x] Design concept
- [ ] Create CreatureAgent.cs (base class)
- [ ] Create HerbivoreAgent.cs
- [ ] Create PredatorAgent.cs
- [ ] Build terrain with food spawning
- [ ] Add reproduction mechanics
- [ ] Long-term training run

---

## Design

### Concept
A living ecosystem where different species compete for resources. Watch predator-prey dynamics, herding behavior, and survival strategies emerge.

### Species

| Species | Diet | Speed | Observation Range | Special |
|---------|------|-------|-------------------|---------|
| **Herbivore** | Plants | Medium | Medium | Can reproduce |
| **Predator** | Herbivores | Fast | Wide | Stamina-limited sprint |
| **Plant** | N/A | Static | N/A | Respawns over time |

### Observations
| Agent | Observations |
|-------|--------------|
| **Both** | Position, energy level, health |
| **Both** | Nearby food sources (raycast or grid) |
| **Both** | Nearby predators/prey (raycast) |
| **Herbivore** | Distance to nearest predator |
| **Predator** | Distance to nearest prey, stamina |

### Actions
| Type | Description |
|------|-------------|
| Continuous | Move direction (x, z), rotation |
| Discrete | Eat, Sprint (predator), Reproduce |

### Energy System
```
Energy depletes over time
Eating restores energy
Reproduction costs energy
Energy = 0 → Death
```

### Rewards
| Event | Herbivore | Predator |
|-------|-----------|----------|
| Eat food/prey | +1.0 | +2.0 |
| Survive per step | +0.001 | +0.001 |
| Reproduce | +5.0 | +5.0 |
| Die (starve) | -1.0 | -1.0 |
| Get eaten | -2.0 | N/A |

### Emergent Behaviors to Watch For
- Herding in herbivores (safety in numbers)
- Pack hunting in predators
- Territorial behavior
- Optimal foraging patterns
- Population boom/bust cycles

---

## Environment Design

### Arena
- Large terrain (100×100 units)
- Grass patches that regrow over time
- Obstacles (rocks, trees) for cover
- Water areas (optional barrier)

### Population Control
- Start with 20 herbivores, 5 predators
- Reproduction when energy > threshold
- Soft cap via food scarcity

---

## Training Strategy
- **MA-POCA** for each species separately
- **Very long training** (5M+ steps) for ecosystem dynamics
- Consider **population-based training** to evolve both species

---

## Metrics to Track
- Population over time (both species)
- Average lifespan
- Reproduction rate
- Prey escape success rate

---

## Next Steps
1. [ ] Create terrain with grass spawning system
2. [ ] Implement energy/health mechanics
3. [ ] Create basic herbivore that eats and moves
4. [ ] Add predator with hunting behavior
5. [ ] Add reproduction system
6. [ ] Run long-term simulation and observe
