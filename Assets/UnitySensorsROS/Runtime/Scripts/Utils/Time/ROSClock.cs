using System;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Rosgraph;

namespace UnitySensors.ROS.Utils.Time
{
    public class ROSClock : MonoBehaviour
    {
        [SerializeField]
        private string _topicName = "clock";

        private ROSConnection _ros;
        private ClockMsg _message;

        private void Start()
        {
            this._ros = ROSConnection.GetOrCreateInstance();

            this._ros.RegisterPublisher<ClockMsg>(this._topicName);

            this._message = new ClockMsg();
            this._message.clock.sec = 0;
            this._message.clock.nanosec = 0;
        }

        private void Update()
        {
            float time = UnityEngine.Time.time;
#if ROS2
            int sec = (int)Math.Truncate(time);
#else
            uint sec = (uint)Math.Truncate(time);
#endif
            uint nanosec = (uint)((time - sec) * 1e+9);
            _message.clock.sec = sec;
            _message.clock.nanosec = nanosec;

            _ros.Publish(this._topicName, this._message);
        }
    }
}