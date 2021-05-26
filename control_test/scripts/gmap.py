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

def clbk_odom(msg):
    global vel_pub
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
    t = geometry_msgs.msg.TransformStamped()
    t.header.frame_id = "odom"
    t.header.stamp = rospy.Time.now()
    t.child_frame_id = "base_link"
    t.transform.translation = msg.pose.position
    t.transform.rotation = msg.pose.orientation

    tfm = tf2_msgs.msg.TFMessage([t])
    vel_pub.publish(tfm)
    

def main():
    global vel_pub
    global i 
    i = 0
    rospy.init_node("simple_motion")

    #defining pubs and subs
    odom_sub = rospy.Subscriber("/pose", geometry_msgs.msg.PoseStamped, clbk_odom)
    vel_pub = rospy.Publisher("/tf", tf2_msgs.msg.TFMessage, queue_size=100)

    r =rospy.Rate(20)
    while not rospy.is_shutdown():
        # Run this loop at about 10Hz
        r.sleep()
    #rospy.spin()
        
if __name__=="__main__":
    main()





