using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{

    public class TimerInfo
    {
        public long tick;
        public bool stop;
        public bool delete;
        public Object target;
        public string className;

        public TimerInfo(string className, Object target)
        {
            this.className = className;
            this.target = target;
            delete = false;
        }
    }
    public class TimerManager : MonoBehaviour
    {
        private float interval = 0;
        private List<TimerInfo> objects = new List<TimerInfo>();

        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        // Use this for initialization
        void Start()
        {
            StartTimer(Const.TimerInterval);
        }

        //启动定时器
        public void StartTimer(float value)
        {
            interval = value;
            InvokeRepeating("Run", 0, interval);
        }

        public void StopTimer()
        {
            CancelInvoke("Run");
        }

        public void AddTimerEvent(TimerInfo info)
        {
            if(!objects.Contains(info))
            {
                objects.Add(info);
            }
        }

        public void RemoveTimerEvent(TimerInfo info)
        {
            if(objects.Contains(info) && info != null)
            {
                info.delete = true;
            }
        }

        public void StopTimerEvent(TimerInfo info)
        // Update is called once per frame
        {
            if(objects.Contains(info) && info != null)
            {
                info.stop = true;
            }
        }

        public void ResumeTimerEvent(TimerInfo info)
        {
            if(objects.Contains(info) && info != null)
            {
                info.delete = false;
            }
        }

        void Run()
        {
            if (objects.Count == 0)
                return;
            for(int i = 0; i < objects.Count; i++)
            {
                TimerInfo o = objects[i];
                if (o.delete || o.stop)
                    continue;
                TimerBehaviour timer = o.target as TimerBehaviour;
                timer.TimerUpdate();
                o.tick++;
            }
            for(int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].delete)
                    objects.Remove(objects[i]);
            }
        }
        void Update()
        {

        }
    }
}

