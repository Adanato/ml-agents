# How to Talk About RL: A Design Framework

When discussing Reinforcement Learning experiments, researchers and engineers typically structure their thinking around the **MDP (Markov Decision Process)** tuple: **(S, A, R, $\gamma$)**.

Use this guide to structure your design notes and hypothesis testing.

---

## 1. The Observation Space (S) | "What does the agent see?"

**Key Questions:**
- **Full vs. Partial Observability:** Does the agent see the whole world (Chess) or just its local view (Tank Arena)?
- **Vector vs. Visual:** Are you feeding it raw numbers (Raycasts, positions) or pixels (Camera)?
- **Temporal Context:** Does it need to remember the past? (Stacking vectors, or using LSTM/Memory)

**How to discuss it:**
> *"I chose a **partially observable** vector space with **raycasts** because visual observations were too computationally expensive. I added **stacking** (3 frames) so it could infer velocity from position changes."*

---

## 2. The Action Space (A) | "What can the agent do?"

**Key Questions:**
- **Continuous:** Smooth values like throttle (-1.0 to 1.0). Best for physics.
- **Discrete:** Multiple choice (Jump vs. Duck). Best for high-level logic.
- **Masking:** Can all actions be taken at all times? (e.g., can't "fire" if cooldown is active).

**How to discuss it:**
> *"I used **continuous actions** for movement to allow smooth turning, but a **discrete branch** for shooting. I applied **action masking** to prevent the agent from spamming the fire button during cooldown."*

---

## 3. The Reward Function (R) | "What do we want?"

This is the hardest part. Rewards guide the gradient descent.

**Key Terms:**
- **Sparse Reward:** Getting points only at the end (e.g., Winning +1, Losing -1). Hard to learn.
- **Dense/Shaped Reward:** Getting points frequently (e.g., +0.1 for moving towards enemy). speeds up learning but risks **Reward Hacking**.
- **Reward Hacking:** The agent finding a loophole (e.g., spinning in circles to collect movement points without fighting).

**How to discuss it:**
> *"Initially, I used a **sparse reward** for winning, but the agent wouldn't explore. I added a **dense shaping reward** for closing distance, but it started **reward hacking** by just following the enemy without shooting. I fixed this by adding a penalty for being too close without firing."*

---

## 4. The Algorithm & Hyperparameters

**Key Terms:**
- **PPO (Proximal Policy Optimization):** The standard "workhorse". Stable, reliable.
- **SAC (Soft Actor-Critic):** Better for complex physics/continuous control. Sample efficient.
- **Gamma ($\gamma$):** Discount factor (0 to 1). How much does the agent care about the future?
  - `0.99` = Long-term thinker (Chess).
  - `0.90` = Short-term thinker (FPS reflex).
- **Beta ($\beta$):** Entropy regularization. How random should it be?
  - High beta = More exploration (try crazy stuff).
  - Low beta = More exploitation (stick to what works).

**How to discuss it:**
> *"The agent converged to a suboptimal strategy too quickly. I increased **Beta** to `0.01` to force more **exploration**, and increased **Gamma** to `0.995` so it would value the long-term goal of survival over the short-term gain of hiding."*

---

## 5. Experimental Design | "Scientific Method"

**The "Ablation Study":** 
To prove a feature works, you train TWO runs: one with it, one without.
- *Run A:* With LSTM Memory.
- *Run B:* Without Memory.
- *Result:* If A beats B consistently, Memory is "necessary".

**The "Curriculum":**
Start simple, get harder.
- *Lesson 1:* Enemy doesn't move.
- *Lesson 2:* Enemy moves randomly.
- *Lesson 3:* Enemy fights back.

**How to discuss it:**
> *"I performed an **ablation study** on the raycast sensors. Removing the 'tag' tag meant the agent couldn't distinguish friends from foes, causing performance to drop by 40%. This confirms the necessity of that signal."*
