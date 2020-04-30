# FYP-ProjectToasty
Final year project

## Game Controls
### Teleportation
- Push right controller **analog stick**
- Press **trigger button** to select

### Snap Turn
- Push left controller **analog stick to right or left** to snap turn

### UI
- Point the teleportation ray at UI and select with trigger **(Currently doesn't work due to 2019.3 bug)**
- See FYP-ProjectToasty\Builds\Full Build\Recorded Bug - Prevents Menu Use

### Grabbing Objects (Pan, Uncracked Egg, Cracked Egg, Bread)
- Hold your controller in close proximity and use the **grip button**


## ML-Agents
**To get setup follow these instructions:**

From Powershell
- python pip install ml-agents

Creating a virtual environment
- virtualenv {Name of venv}
- navigate to \path\to\env\Scripts\activate to activate

Running ML Agents
- Navigate to the root directory of your project
- Run the following command, "mlagents-learn config/gail_config.yaml --run-id=Custom_ID --train"
- Change the .yaml file to correspond with your own custom parameter settings for RL
- Change the run-id everytime you train to create a distinction between trained models. Tensorboard will also show this distinction.

Running Tensorboard for Training Statistics
- Open a terminal or conole window
- Naviagte to the project directory
- From the command line run: tensorboard --logdir=results --port=6006
- Open a browser window and nacigate to localhost:6006

**Unity Version:** 2019.3
