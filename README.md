# GoodmintonRL
Reinforcement Learning - CS486 - Final Project

By Jesse Rivera, Colin Page, Peter Cooke

# Installation (Windows):
1. Install 6000.0.25f1 through Unity Hub
2. Install Python version 3.10.11
3. Install Python dependencies:

For this step, we recommend referencing the official Unity ML-Agents Toolkit installation instructions here: https://unity-technologies.github.io/ml-agents/Installation/ 

Here is a summary of that page:

  * Install PyTorch by running the following command: ```pip3 install torch~=2.2.1 --index-url https://download.pytorch.org/whl/cu121```
    
  * Clone the ml-agents repository: https://github.com/Unity-Technologies/ml-agents
    
  * Navigate to the cloned ml-agents, and install the two included python packages from it using pip:

        cd /path/to/ml-agents
    
        python -m pip install ./ml-agents-envs
    
        python -m pip install ./ml-agents
    
      
4. Clone this repository onto your computer

# Opening and Running the Project:
1. Open the Unity project from Unity Hub
2. From the Unity project window, open the "Court" scene, found in Assets/Scenes
  * Playing this scene will show multiple instances of the agent playing using its trained policy
3. Open and run the "HumanTest" scene to play interactively with the trained agent using WASD controls

# Running training:
1. Open the Court scene in Unity
2. Open a command window in .../BadmintonRL/Goodminton/Assets/
3. Initiate training by running this command: ```mlagents-learn badminton_config.yaml --run-id=BadmintonX```
4. Hit the Play button in the Unity editor to begin training
5. Training will stop after 500000 steps, or when you stop the Unity scene or cancel the command
6. View the training results by running this command: ```tensorboard --logdir results --port 6006```, then visit localhost:6006 in your browser
