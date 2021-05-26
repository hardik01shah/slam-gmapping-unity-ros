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
The robot in this project is just a simple cylinder to emulate a LIDAR. 

1. The _lidarfinal_ script simulates a LIDAR in Unity using the raycast feature.
    >In the _Scene_ tab of the Unity GUI you can see the rays emanating from the LIDAR. This has been done using ray.debug feature. </br>
    >Refer to https://github.com/hardik01shah/Lidar-Simulation-Using-Unity-ROS
2. The _Laser_Scan_Publisher_ script of ROS# publishes the LIDAR data of _lidarfinal_ to the _/scan_temp_ rostopic.
    > LIDAR data on the _/scan_temp_ topic does not have the timestamp correct and it causes errors in RViZ if we directly use this data. The timestamp is fixed
    > using a python script on the ROS side.
3. The _Pose_Stamped_Publisher_ script of ROS publishes the pose of the robot on _/pose_ topic.
4. There are two scripts available for moving the bot:
    - _teleop.cs_  
      This script takes in velocity commands from the user directly through UNITY using WASD.
    - _updown.cs_  
      This script takes velocity commands from the _Twist_Subscriber_ script of ROS# which recieves velocity commands from the _/cmd_vel_ topic.
    >Both the above scripts can be used however, a more realistic simulation would use the _updown.cs_ script. The video below however makes use of the
    >_teleop.cs_ script to reduce the load on the CPU while recording.

## Setup on ROS side:
1. Launch rosbridge:
    ```sh
    roslaunch rosbridge_server rosbridge_websocket.launch
    ```
2. run _scan_fix.py_ :(make sure you source the terminal window first)
    ```sh
    source devel/setup.bash
    ./scan_fix.py
    ```
    >This script fixes the timestamp of the _laserscan_ data recieved from Unity.
3. run _gmap.py_ :
    ```sh
    source devel/setup.bash
    ./gmap.py
    ```
    >This script subscribes to the _/pose_ topic and establishes a trnsform from the map->odom frame.
4. Establish transform from the base_link to base_scan:
    ```sh
    rosrun tf2_ros static_transform_publisher 0 0 0 0 0 0 1 /base_link /base_scan
    ```
    >This is a static transform publisher. Since there is no relative motion between the lidar and the robot body, a static transform 
    >publisher is used.
5. Launch turtlebot3 slam mapping and kill the process immediately after starting it:
    ```sh
    export TURTLEBOT3_MODEL=waffle_pi
    roslaunch turtlebot3_slam turtlebot3_slam.launch slam_methods:=gmapping
    ```
    >This step is optional. The gmapping will work even without this step. However, we won't be able to visualize the robot model in RViZ
    >without this. I haven't written a joint_state_pubisher so this command takes care of that.</br>
    >Kill this process immediately after starting it using Ctrl + C.
6. Launch gmapping:
    ```sh
    rosrun gmapping slam_gmapping _linearUpdate:=0.0 _angularUpdate:=0.0
    ```
    >linearUpdate and angularUpdate are parameters. When set to zero gmapping will continue to execute even when the bot is stationary.
7. Launch RViZ:
    ```sh
    rosrun rviz rviz
    ```
    >Add Map(/map), LaserScan(/scan), RobotModel to the config. 
8. Save the map after gmapping is complete:
    ```sh
    rosrun map_server map_saver
    ```
    >A .pgm and .yaml file is saved.

## Simulation:
https://user-images.githubusercontent.com/61026273/119598940-93de5b80-be01-11eb-80e5-bb9aa4f787a7.mp4

The rqt_graph:</br>
![rosgraph_unity_gmap](https://user-images.githubusercontent.com/61026273/119598976-ad7fa300-be01-11eb-9eb9-9d0ce6255dbe.png)

The tf_tree:</br>
![frames_unity_gmap](https://user-images.githubusercontent.com/61026273/119598996-b53f4780-be01-11eb-8522-c2c0e78b1d67.png)

## Files:
1. Lidar Testing is the complete Unity Project.
2. control_test is the project folder from the catkin_ws
3. Map generated after gmapping has also been uploaded.

## Error Handling:
1. Time stamp delay between Unity and ROS. When LaserScan data is published from Unity to ROS, and when that data is used by SLAM and visualized in RViZ, there is a delay and that eventually leads to the error “Message is too old to display”. This was handled using a buffer script that assigns a new timestamp to the incoming message from Unity to stop the delay.
2. The transform between the map and odom frame does not exist. This error does not show up in Gazebo because it already takes care of configuring the transform of the bot using the incoming odometry data. However, in the case of Unity a transform must be created between the map frame and the odom frame using the pose of the bot. This is again done using a rospy script that subscribes to the /pose topic that publishes the pose of the bot in Unity. The script manually assigns a transform between the map and odom frame.
3. There must exist a transform between the robot_base and the base_scan. This means the relative position of the LiDAR w.r.t the base of the robot. Since, there is no relative motion between the LiDAR and the robot base a static_tranform_publisher is used to create this transform. This is done natively in another terminal window. There is no need for a script for this task. The static_tranform_publisher of ROS handles this. 
4. The LiDAR data received is not updated. This occurs because what we are trying to do is a simulation. The parameters of the LiDAR are ideal which is not possible in the real world. The time_update parameter of the LiDAR was initially set to 0 which is an ideal value, not achievable in the real world. This error was handled by setting the time_update value of a real LiDAR available in the market. 
