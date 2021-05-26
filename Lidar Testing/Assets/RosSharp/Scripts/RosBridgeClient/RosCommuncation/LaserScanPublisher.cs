/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LaserScanPublisher : Publisher<Messages.Sensor.LaserScan>
    {
        //public LaserScanReader laserScanReader;
        public string FrameId = "base_scan";

        private Messages.Sensor.LaserScan message;
        private float scanPeriod;
        private float previousScanTime = 0;
                
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            if (Time.realtimeSinceStartup >= previousScanTime + scanPeriod)
            {
                UpdateMessage();
                previousScanTime = Time.realtimeSinceStartup;
            }
        }

        private void InitializeMessage()
        {
            scanPeriod = 0;

            message = new Messages.Sensor.LaserScan
            {
                header = new Messages.Standard.Header { frame_id = FrameId },
                angle_min       = 0.0f,
                angle_max       = 6.28318977356f,
                angle_increment = 0.0175019223243f,
                time_increment  = 1,
                range_min       = 0,
                range_max       = 100.0f,
                ranges          = lidarfinal.distances,      
                intensities     = {}

            };
        }

        private void UpdateMessage()
        {
            message.header.Update();
            message.ranges = lidarfinal.distances;
            Publish(message);
        }
    }
}
