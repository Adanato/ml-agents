# ML-Agents Setup Notes (titan-xp-ubuntu)

## System Configuration

| Component | Version/Details |
|-----------|-----------------|
| **Machine** | titan-xp-ubuntu |
| **GPUs** | 2× NVIDIA TITAN Xp (12 GB each, Pascal sm_61) |
| **Python** | 3.10.12 (required by ml-agents) |
| **Package Manager** | uv (Astral) |
| **Unity** | 6000.0.40f1 |

---

## Environment Setup

### Create Virtual Environment
```bash
cd ~/Research/ml-agents
uv venv --python 3.10.12
```

### Install ML-Agents (Editable Mode)
```bash
VIRTUAL_ENV=.venv uv pip install -e ./ml-agents-envs -e ./ml-agents
```

### GPU Compatibility Fix (Pascal Architecture)
Default PyTorch 2.8.0 doesn't support sm_61. Downgrade to 2.3.1+cu118:
```bash
VIRTUAL_ENV=.venv uv pip install "torch==2.3.1" "torchvision" "torchaudio" --index-url https://download.pytorch.org/whl/cu118
```

### Verify GPU Support
```bash
.venv/bin/python -c "import torch; print(f'GPUs: {torch.cuda.device_count()}, Compatible: {torch.cuda.is_available()}')"
```

---

## Training Workflow

### Start Training
```bash
.venv/bin/mlagents-learn config/ppo/3DBall.yaml --run-id=my_run --force
# Then press Play in Unity Editor
```

### Resume Training
```bash
.venv/bin/mlagents-learn config/ppo/3DBall.yaml --run-id=my_run --resume
```

### Monitor with TensorBoard
```bash
.venv/bin/tensorboard --logdir results
# Open http://localhost:6006
```

---

## Inference Workflow

### Python-side Inference (Recommended)
```bash
.venv/bin/mlagents-learn config/ppo/3DBall.yaml --run-id=my_run --resume --inference
# Press Play in Unity - uses trained model automatically
```

### Unity-side Inference
1. Copy `.onnx` file to `Assets/.../TFModels/`
2. Drag onto Agent's `Model` field in Behavior Parameters
3. Set `Behavior Type` → `Inference Only`

---

## Headless Training (Server/Cluster)

### Build Headless Executable
1. Unity: `File → Build Settings → Linux → Server Build ✓`
2. Build to `./builds/TankArena/`

### Run Multi-Instance Training
```bash
.venv/bin/mlagents-learn config/ppo/TankArena.yaml \
  --run-id=tank_headless \
  --env=./builds/TankArena/TankArena.x86_64 \
  --num-envs=8 \
  --no-graphics \
  --force
```

### Slurm Job Example
```bash
#!/bin/bash
#SBATCH --job-name=tank_train
#SBATCH --gres=gpu:1
#SBATCH --time=24:00:00

cd ~/Research/ml-agents
source .venv/bin/activate
mlagents-learn config/ppo/TankArena.yaml --run-id=tank_slurm --env=./builds/TankArena.x86_64 --num-envs=8 --no-graphics
```

---

## Troubleshooting

### UnityTimeOutException
Python trainer waited too long for Unity connection.
- Press Play in Unity after seeing "Listening on port 5004"
- Use `--timeout-wait=120` for slow startups

### GLIBC Errors on Cluster
Binary compiled on newer glibc won't run on older systems.
- Check: `ldd --version`
- Solution: Build on older system or use containers

### GPU sm_61 Not Supported
PyTorch 2.8.0 dropped Pascal support.
- Solution: Use PyTorch 2.3.1+cu118 (see above)
