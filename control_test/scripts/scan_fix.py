#! /usr/bin/env python

import sys
import math
import rospy
'''
from geometry_msgs.msg import Twist, Point, Quaternion, PoseStamped, TransformStamped, Vector3
from nav_msgs.msg import Odometry
from std_msgs.msg import Header
from tf2_msgs.msg import TFMessage
'''
import rospy
import tf2_ros
import tf2_msgs.msg
import geometry_msgs.msg
import sensor_msgs.msg

def clbk_odom(msg):
    global vel_pub
    global i
    ''' 
    s = geometry_msgs.msg.TransformStamped()
    s.header.frame_id = "map"
    s.header.stamp = rospy.Time.now()
    s.child_frame_id = "odom"
    tmp1 = msg.pose.position
    tmp1.x = 0.0
    tmp1.y = 0.20
    tmp1.z = 0.0
    tmp2 = msg.pose.orientation
    tmp2.x = 0.0
    tmp2.y = 0.0
    tmp2.z = 0.0
    tmp2.w = 1.0
    s.transform.translation = tmp1
    s.transform.rotation = tmp2
    '''
    t = msg
    t.header.stamp = rospy.Time.now()
    t.header.seq = i
    i = i+1
    t.header.frame_id = "base_scan"
    t.time_increment = 1.73611115315e-05
    t.scan_time = 0.006250000003725
    vel_pub.publish(t)
    

def main():
    global vel_pub
    global i 
    i = 0
    rospy.init_node("scan_FIXER")

    #defining pubs and subs
    odom_sub = rospy.Subscriber("/scan_temp", sensor_msgs.msg.LaserScan, clbk_odom)
    vel_pub = rospy.Publisher("/scan", sensor_msgs.msg.LaserScan, queue_size=100)

    r =rospy.Rate(20)
    while not rospy.is_shutdown():
        # Run this loop at about 10Hz
        r.sleep()
    #rospy.spin()
        
if __name__=="__main__":
    main()





