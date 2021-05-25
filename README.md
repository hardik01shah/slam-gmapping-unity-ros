# Slam Gmapping in Unity-ROS
Executing SLAM gmapping in Unity ROS setup

## Overview:
This repository contains all the necessary steps and packages required for setting up slam gmapping in the Unity ROS setup.</br>
For basic Unity ROS setup, refer [here](https://github.com/hardik01shah/Unity-ROS-Basic-Simulation).
</br>
SLAM (Simultaneous Localization and Mapping) uses various information about the state of the robot and then creates a map of the environment. There are various methods for SLAM like gmapping, cartography, hector mapping etc. For this project SLAM gmapping is used, however other methods can also be used according to the user requirements. SLAM gmapping in ROS mainly requires two things:
1. LaserScan Data 
2. Transform from the map frame to the odom frame
</br
## Packages:
1. [ROS#](https://github.com/siemens/ros-sharp)
2. [SNAPS Prototype|Office Asset](https://assetstore.unity.com/packages/3d/environments/snaps-prototype-office-137490) (from Unity Asset Store)
  >This asset is basically an office environment. It can be directly imported from the Asset Store.

## Setup on UNITY side:
1. The _lidarfinal_ script simulates a LIDAR in Unity using the raycast feature.
2. The _Laser_Scan_Publisher_ script of ROS# publishes the LIDAR data of _lidarfinal_ to the _scan_temp_ rostopic.
3. The _Pose_Stamped_Publisher_ script of ROS publishes the pose of the robot on _/pose_  
teleop vs updown  
cylinder is robot  
add empty gameobject, rosconnector in prev repo.  
add map.pgm in files.  
