# CLAUDE.md — ml-agents

## Overview

Fork of Unity ML-Agents Toolkit. Open-source project for training intelligent agents in Unity environments using RL (PPO, SAC, MA-POCA), imitation learning (BC, GAIL), and self-play. Used for personal experiments with game AI.

## Structure

```
ml-agents/                — Python training package (mlagents)
ml-agents-trainer-plugin/ — Custom trainer plugin interface
ml-agents-plugin-examples/— Example plugins
Project/                  — Unity project with example environments
config/                   — Training configs (ppo/, sac/, poca/, imitation/)
```

## Key Commands

```bash
pip install -e ml-agents/              # Install training package
mlagents-learn config/ppo/3DBall.yaml  # Train an agent
```

## Key Conventions

- Fork of Unity-Technologies/ml-agents (release/4.0.0 branch)
- Python training + C# Unity SDK
- Local modifications to PushBlockWithInput example
- 3 commits behind upstream on develop branch
